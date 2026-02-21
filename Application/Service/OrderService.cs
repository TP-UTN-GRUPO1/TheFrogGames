using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Contracts.MercadoPago.Request;
using Contracts.Order.Request;
using Contracts.Responses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IGameRepository _gameRepo;
    private readonly IMercadoPagoService _mercadoPagoService;

    public OrderService(
        IOrderRepository orderRepo,
        IGameRepository gameRepo,
        IMercadoPagoService mercadoPagoService)
    {
        _orderRepo = orderRepo;
        _gameRepo = gameRepo;
        _mercadoPagoService = mercadoPagoService;
    }
    public List<OrderResponse> GetOrders()
    {
        var orders = _orderRepo
            .FindByCondition(o => !o.IsCancelled, trackChanges: false)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Game)
            .ToList();

        return orders.Select(MapToOrderResponse).ToList();
    }

    public async Task<OrderResponse?> CreateOrder(CreateOrderRequest request, int userId)
    {
        var gameIds = request.Items.Select(i => i.GameId).ToList();
        var gamePrices = _gameRepo.GetPricesByIds(gameIds);

        if (gamePrices.Count != gameIds.Count)
            throw new Exception("Uno o mas juegos no estan disponibles o no se encontraron");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 0m,
        };

        decimal calculatedTotal = 0m;

        foreach (var itemReq in request.Items)
        {
            if (!gamePrices.TryGetValue(itemReq.GameId, out decimal unitPrice))
                continue;

            if (itemReq.Quantity <= 0)
                continue;

            decimal subtotal = itemReq.Quantity * unitPrice;

            var item = new OrderItem
            {
                Order = order,
                GameId = itemReq.GameId,
                Quantity = itemReq.Quantity,
                UnitPrice = unitPrice,
                Subtotal = subtotal
            };

            order.OrderItems.Add(item);
            calculatedTotal += subtotal;
        }

        if (order.OrderItems.Count == 0)
            return null;

        order.TotalAmount = calculatedTotal;

        bool created = _orderRepo.Create(order);
        if (!created) return null;

        var createdOrder = _orderRepo.GetOrderWithItems(order.Id, trackChanges: false);
        if (createdOrder == null) return null;

       
        var preferenceRequest = new CreatePreferenceRequest
        {
            Items = createdOrder.OrderItems.Select(i => new MPItem
            {
                Title = i.Game?.Title ?? "Game",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var checkout = await _mercadoPagoService.CreatePreferenceAsync(preferenceRequest);

        var response = MapToOrderResponse(createdOrder);

        
        response.CheckoutUrl = checkout.CheckoutUrl;

        return response;
    }

    public OrderResponse? GetOrderById(int id)
    {
        var order = _orderRepo.GetOrderWithItems(id, trackChanges: false);
        if (order == null) return null;
        return MapToOrderResponse(order);
    }

    public List<OrderResponse> GetOrdersByUser(int userId)
    {
        var orders = _orderRepo.GetOrdersByUser(userId, trackChanges: false);
        return orders.Select(MapToOrderResponse).ToList();
    }

    public bool DeleteOrder(int id)
    {
        var order = _orderRepo.GetById(id, trackChanges: true);
        if (order == null) return false;

        if (order.IsCancelled) return true;

        order.IsCancelled = true;
        return _orderRepo.Update(order);
    }

    private OrderResponse MapToOrderResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Total = order.TotalAmount,
            Items = order.OrderItems.Select(i => new OrderItemResponse
            {
                GameId = i.GameId,
                GameTitle = i.Game?.Title,
                Developer = i.Game?.Developer,
                ImageUrl = i.Game?.ImageUrl,
                Price = i.Game?.Price ?? 0,
                Rating = i.Game?.Rating ?? 0,
                Available = i.Game?.Available ?? false,
                Sold = i.Game?.Sold ?? 0,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Subtotal
            }).ToList(),
            IsCancelled = order.IsCancelled
        };
    }
}