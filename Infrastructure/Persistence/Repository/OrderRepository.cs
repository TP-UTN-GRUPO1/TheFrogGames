using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(TheFrogGamesDbContext context) : base(context)
        {
        }

        public List<Order> GetOrdersByUser(int userId, bool trackChanges = false)
        {
            var query = _context.Set<Order>()
                                .Where(o => o.UserId == userId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public Order? GetOrderWithItems(int orderId, bool trackChanges = false)
        {
            var query = _context.Set<Order>()
                                .Include(o => o.Items)
                                .Where(o => o.Id == orderId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }
    }
}

