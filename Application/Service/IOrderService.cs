using System.Collections.Generic;
using Contracts.Order.Request;
using Contracts.Responses;

public interface IOrderService
{
    List<OrderResponse> GetOrders();
    OrderResponse? CreateOrder(CreateOrderRequest request, int userId);
    OrderResponse? GetOrderById(int id);
    List<OrderResponse> GetOrdersByUser(int userId);
    bool DeleteOrder(int id);
   
}
