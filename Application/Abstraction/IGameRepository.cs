using Domain.Entities;

namespace Application.Abstraction
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game?> GetByIdAsync(int id);
        Game? GetByIdWithRelations(int id, bool trackChanges = false);
        Dictionary<int, decimal> GetPricesByIds(List<int> gameIds);
        Task AddAsync(Game game);
        Task SaveChangesAsync();
        Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken);
    }
}
