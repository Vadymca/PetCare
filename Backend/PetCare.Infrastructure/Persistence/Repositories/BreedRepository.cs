
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        public BreedRepository(AppDbContext context) 
            : base(context) { }

        public async Task<IReadOnlyList<Breed>> GetBySpeciesIdAsync(
            Guid speciesId, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Breeds
                .Where(b =>  b.SpeciesId == speciesId)
                .ToListAsync(cancellationToken);
        }
    }
}
