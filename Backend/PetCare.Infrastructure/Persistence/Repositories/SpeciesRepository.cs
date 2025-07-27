

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class SpeciesRepository : GenericRepository<Specie>, ISpeciesRepository
    {
        public SpeciesRepository(AppDbContext context) 
            : base(context) { }

        public async Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Species
                .FirstOrDefaultAsync(s => s.Name.Value == name, cancellationToken);
        }
    }
}
