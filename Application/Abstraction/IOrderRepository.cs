using Domain.Entities;
namespace Application.Abstraction
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        List<Order> GetOrdersByUser(int userId, bool trackChanges = false);
        Order? GetOrderWithItems(int orderId, bool trackChanges = false);

    }

}
