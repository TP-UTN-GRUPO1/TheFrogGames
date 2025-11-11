using System.Linq.Expressions;

namespace Application.Abstraction
{
    public interface IBaseRepository<T> where T : class 
    {
        List<T> GetAll(bool trackChanges = false);
        T? GetById(object id, bool trackChanges = false);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        List<T> GetByCriteria(Expression<Func<T, bool>> expression);

        int Save();

        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
    }
}
