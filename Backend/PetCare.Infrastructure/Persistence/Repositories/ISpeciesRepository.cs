using PetCare.Domain.Entities;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public interface ISpeciesRepository : IRepository<Specie>
    {
        Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
