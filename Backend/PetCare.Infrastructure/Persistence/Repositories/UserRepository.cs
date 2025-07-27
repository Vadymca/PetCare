using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;

namespace PetCare.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) 
            : base(context) { }

        public async Task<User?> GetByEmailAsync(
            string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => 
                u.Email.Value == email, cancellationToken);
        }
    }
}
