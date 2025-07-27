using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(
            string email, CancellationToken cancellationToken = default);
    }
}
