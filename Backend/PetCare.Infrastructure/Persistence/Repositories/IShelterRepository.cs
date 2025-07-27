using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public interface IShelterRepository : IRepository<Shelter>
    {
        Task<Shelter?> GetBySlugAsync(
            string slug, CancellationToken cancellationToken = default);
    }
}
