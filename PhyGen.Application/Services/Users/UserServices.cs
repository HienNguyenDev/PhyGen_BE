using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.DTOs;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;
using PhyGen.Domain.Users.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtProvider _jwtProvider;

        public UserServices(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result> CreateUser(UserDTO userDTO, CancellationToken cancellationToken)
        {
            var identityId = await _authenticationService.RegisterAsync(userDTO.Email, userDTO.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDTO.UserName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                IdentityId = identityId,
            };
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success("User created successfully.");
        }

        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure<User>(UserErrors.NotFoundByEmail);
            }
            return user;
        }

        public async Task<Result<User>> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return Result.Failure<User>(UserErrors.NotFoundByUsername);
            }
            return user;
        }

        public async Task<Result> GetUsersByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            if (users == null || !users.Any())
            {
                return Result.Failure(UserErrors.NotFoundByRole);
            }
            return Result.Success(users);
        }

        //public async Task<List<User>> GetActiveUsersAsync()
        //{
        //    return await _userRepository.GetActiveUsersAsync();
        //}

        public async Task<Page<User>> GetUsersPaginatedAsync(
            int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null)
        {
            return await _userRepository.GetUsersPaginatedAsync(
                pageNumber,
                pageSize,
                roleFilter,
                statusFilter,
                searchTerm);
        }

        public async Task<Result> UpdateUser(User user, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success("User updated successfully.");
        }

        public async Task<Result> InActiveUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.Status = UserStatus.Inactive;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success("User inactive successfully.");
        }

        public async Task<string> LoginUser(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            return await _jwtProvider.GetForCredentialsAsync(loginDTO.Email,
                loginDTO.Password);
        }
    }
}
