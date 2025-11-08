
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

        public List<Order> GetAllWithItems(bool trackChanges = false)
        {
            IQueryable<Order> baseQuery = _context.Set<Order>();

            if (!trackChanges)
                baseQuery = baseQuery.AsNoTracking();

            return baseQuery
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Game)
                .ToList();
        }

        public List<Order> GetOrdersByUser(int userId, bool trackChanges = false)
        {
            // 1. Crea la consulta base
            IQueryable<Order> baseQuery = _context.Set<Order>()
                .Where(o => o.UserId == userId);

            if (!trackChanges)
                baseQuery = baseQuery.AsNoTracking();

            return baseQuery
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Game)
                .ToList();
        }
        public Order? GetOrderWithItems(int orderId, bool trackChanges = false)
        {
            IQueryable<Order> baseQuery = _context.Set<Order>()
                .Where(o => o.Id == orderId);
            if (!trackChanges)
                baseQuery = baseQuery.AsNoTracking();
            return baseQuery
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Game)
                .FirstOrDefault();
        }
    }
}

