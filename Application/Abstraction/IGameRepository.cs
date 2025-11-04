using Domain.Entities;

namespace Application.Abstraction
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game?> GetByIdAsync(int id);
        Task AddAsync(Game game);
        Task SaveChangesAsync();
        Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken);

    }
}
