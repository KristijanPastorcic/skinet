using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification);
    Task<IReadOnlyList<T>> ListAllWithSpecAsync(ISpecification<T> specification);
    Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> specification);
    Task<IReadOnlyList<TResult>> ListAllWithSpecAsync<TResult>(ISpecification<T,TResult> specification);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> saveAllAsync();
    bool Exists(int id);
}
