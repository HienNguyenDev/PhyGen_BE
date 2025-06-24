using PhyGen.Application.Common;
using PhyGen.Application.DTOs.Authentication;
using PhyGen.Application.DTOs.Users;
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
        Task<ApiResult<User>> CreateUser(RegisterUserDTO userDTO, CancellationToken cancellationToken);
        Task<ApiResult<User>> GetCurrentUser(CancellationToken cancellationToken);
        Task<ApiResult<User>> GetByEmailOrUsernameAsync(string emailOrUsername);
        Task<List<User>> GetUsersByRoleAsync(UserRole role);
        Task<ApiResult<Page<User>>> GetUsersPaginatedAsync(
            int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null);
        Task<ApiResult<string>> UpdateUser(Guid id, UpdateUserDTO updateUserDTO, CancellationToken cancellationToken);
        Task<ApiResult<string>> InActiveUser(Guid userId, CancellationToken cancellationToken);
        Task<ApiResult<TokenResponse>> LoginUser(LoginDTO loginDTO, CancellationToken cancellationToken);
        Task<string> LoginWithFirebaseUidAsync(string uid, CancellationToken cancellationToken);
    }
    
}
