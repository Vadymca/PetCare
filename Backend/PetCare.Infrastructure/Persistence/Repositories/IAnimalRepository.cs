using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        Task<Animal?> GetBySlugAsync(
            string slug, CancellationToken cancellationToken = default);
    }
}
