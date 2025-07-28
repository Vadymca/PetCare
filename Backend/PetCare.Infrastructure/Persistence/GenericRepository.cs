using Microsoft.EntityFrameworkCore;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
            
        }
        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }


    }
}
