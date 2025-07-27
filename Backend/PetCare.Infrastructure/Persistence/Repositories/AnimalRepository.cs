using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(AppDbContext context) 
            : base(context) { }


        public async Task<Animal?> GetBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Animals
                .FirstOrDefaultAsync(a => a.Slug.Value == slug, cancellationToken);
        }
    }
}
