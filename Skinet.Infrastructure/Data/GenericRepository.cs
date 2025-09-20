using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.Infrastructure.Data
{
    public class GenericRepository<T>(StoreContext storeContext) : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext = storeContext;

        public void Add(T entity)
        {
            _storeContext.Set<T>().Add(entity);
        }

        public bool Exists(int id)
        {
            return _storeContext.Set<T>().Any(x => x.Id == id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _storeContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecificaiton<T, TResult> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _storeContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecificaiton<T, TResult> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public void Remove(T entity)
        {
            _storeContext.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _storeContext.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _storeContext.Set<T>().Attach(entity);
            _storeContext.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_storeContext.Set<T>().AsQueryable(), spec);
        }

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecificaiton<T, TResult> spec)
        {
            return SpecificationEvaluator<T>.GetQuery<T, TResult>(_storeContext.Set<T>().AsQueryable(), spec);
        }
    }
}
