using LGC_CodeChallenge.Models;

namespace LGC_CodeChallenge.Interfaces
{
    public interface IGenericRepository<T> where T : IEntityBase
    {
        Task<T> GetAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
