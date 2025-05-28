using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Extensions;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;

namespace PhyGen.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _dbSet
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.Status == UserStatus.Active)
                .ToListAsync();
        }

        public async Task<Page<User>> GetUsersPaginatedAsync(
            int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null)
        {
            var query = _dbSet.AsQueryable();

            if (roleFilter.HasValue)
                query = query.Where(u => u.Role == roleFilter);

            if (statusFilter.HasValue)
                query = query.Where(u => u.Status == statusFilter);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(u => u.Username.Contains(searchTerm) || 
                                       u.Email.Contains(searchTerm));

            int count = await query.CountAsync();
            var items = await query
                .ApplyPagination(pageNumber, pageSize)
                .ToListAsync();

            return new Page<User>(items, count, pageNumber, pageSize);
        }
    }
}
