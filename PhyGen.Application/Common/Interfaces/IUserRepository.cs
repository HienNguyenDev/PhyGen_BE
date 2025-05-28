using PhyGen.Domain.Common;
using PhyGen.Domain.Users;

namespace PhyGen.Application.Common.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<List<User>> GetActiveUsersAsync();
        Task<Page<User>> GetUsersPaginatedAsync(
            int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null);
    }
}