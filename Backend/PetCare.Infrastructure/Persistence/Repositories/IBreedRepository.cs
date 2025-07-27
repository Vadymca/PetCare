using PetCare.Domain.Entities;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public interface IBreedRepository : IRepository<Breed>
    {
        Task<IReadOnlyList<Breed>> GetBySpeciesIdAsync(
            Guid speciesId, CancellationToken cancellationToken = default);
    }
}
