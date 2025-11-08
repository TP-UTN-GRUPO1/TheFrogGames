using Domain.Entities;

namespace Application.Abstraction
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game?> GetByIdAsync(int id);
        Dictionary<int, decimal> GetPricesByIds(List<int> gameIds);
        Task AddAsync(Game game);
        Task SaveChangesAsync();
        Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken);

    }
}
