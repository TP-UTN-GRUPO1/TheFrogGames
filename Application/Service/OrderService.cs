
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Contracts.Responses;
using Contracts.Order.Request;
using Domain.Entities;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IGameRepository _gameRepo;

    public OrderService(IOrderRepository orderRepo,
                        IGameRepository gameRepo)
    {
        _orderRepo = orderRepo;
        _gameRepo = gameRepo;
    }

    public List<OrderResponse> GetOrders()
    {
        var orders = _orderRepo
            .FindByCondition(o => true, trackChanges: false)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Game)
            .ToList();

        return orders.Select(MapToOrderResponse).ToList();
    }

    public OrderResponse? CreateOrder(CreateOrderRequest request)
    {
        var gameIds = request.Items.Select(i => i.GameId).ToList();
        var gamePrices = _gameRepo.GetPricesByIds(gameIds);

        if (gamePrices.Count != gameIds.Count)
            throw new Exception("Uno o mas juegos no estan disponibles o no se encontraron");

        var order = new Order
        {
            UserId = request.UserId,
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

        return MapToOrderResponse(createdOrder);
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
        var order = _orderRepo.GetById(id, trackChanges: false);
        if (order == null) return false;
        return _orderRepo.Delete(order);
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
                Id = i.Id,
                GameTitle = i.Game?.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}