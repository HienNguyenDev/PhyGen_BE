using PhyGen.Application.DTOs;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Users
{
    public interface IUserServices
    {
        Task<Result> CreateUser(UserDTO UserDTO, CancellationToken cancellationToken);
        Task<Result<User>> GetByEmailAsync(string email);
        Task<Result<User>> GetByUsernameAsync(string username);
        Task<Result> GetUsersByRoleAsync(UserRole role);
        Task<Page<User>> GetUsersPaginatedAsync(int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null);
        Task<Result> UpdateUser(User user, CancellationToken cancellationToken);
        Task<Result> InActiveUser(Guid userId, CancellationToken cancellationToken);
        Task<string> LoginUser(LoginDTO loginDTO, CancellationToken cancellationToken);
    }
    
}
