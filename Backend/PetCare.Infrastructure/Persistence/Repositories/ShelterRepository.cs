using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class ShelterRepository : GenericRepository<Shelter>, IShelterRepository
    {
        public ShelterRepository(AppDbContext context) 
            : base(context) { }

        public async Task<Shelter?> GetBySlugAsync(
            string slug, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Shelters
                .FirstOrDefaultAsync(s => s.Slug.Value == slug, cancellationToken);
        }
    }
}
