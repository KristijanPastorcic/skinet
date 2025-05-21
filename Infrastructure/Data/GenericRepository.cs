using System.Buffers.Text;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext _storeContext) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity) => _storeContext.Set<T>().Add(entity);

    public void Delete(T entity) => _storeContext.Set<T>().Remove(entity);

    public bool Exists(int id) => _storeContext.Set<T>().Any(e => e.Id == id);

    public async Task<T?> GetByIdAsync(int id) => await _storeContext.Set<T>().FindAsync(id);

    public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).FirstAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync() => await _storeContext.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> ListAllWithSpecAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAllWithSpecAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<bool> saveAllAsync() => await _storeContext.SaveChangesAsync() > 0;

    public void Update(T entity)
    {
        _storeContext.Set<T>().Attach(entity);
        _storeContext.Entry(entity).State = EntityState.Modified;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>
            .GetQuery(_storeContext.Set<T>().AsQueryable(), specification);
    }

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T,TResult> specification)
    {
        return SpecificationEvaluator<T>
            .GetQuery<T, TResult>(_storeContext.Set<T>().AsQueryable(), specification);
    }
}
