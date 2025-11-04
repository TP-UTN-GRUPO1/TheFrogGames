using Application.Abstraction;
using Contracts.Responses;
using Contracts.Order.Request;
using Domain.Entities;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IBaseRepository<OrderItem> _orderItemRepo;

    public OrderService(IOrderRepository orderRepo,
                        IBaseRepository<OrderItem> orderItemRepo)
    {
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
    }

    public List<OrderResponse> GetOrders()
    {
        var orders = _orderRepo.GetAll();

        var orderResponseList = orders.Select(o => MapToOrderResponse(o)).ToList();

        return orderResponseList;
    }


    public OrderResponse? CreateOrder(CreateOrderRequest request)
    {
        var order = new Order
        {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow
        };

        foreach (var itemReq in request.Items)
        {
            var item = new OrderItem
            {
                GameId = itemReq.GameId,
                Quantity = itemReq.Quantity,
                UnitPrice = itemReq.UnitPrice
            };

            order.Items.Add(item);
        }

        bool created = _orderRepo.Create(order);
        if (!created) return null;

        var createdOrder = _orderRepo.GetOrderWithItems(order.Id, trackChanges: false);
        if (createdOrder == null) return null;


        var response = MapToOrderResponse(createdOrder);
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
        return orders.Select(o => MapToOrderResponse(o)).ToList();
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
            OrderDate = order.OrderDate,
            Total = order.Total,
            Items = order.Items.Select(i => new OrderItemResponse
            {
                Id = i.Id,
                GameId = i.GameId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
