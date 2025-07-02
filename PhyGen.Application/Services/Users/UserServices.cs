using FirebaseAdmin.Auth;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Application.DTOs.Authentication;
using PhyGen.Application.DTOs.Users;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserContext _userContext;

        public UserServices(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IDateTimeProvider dateTimeProvider, IUserContext userContext)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
            _dateTimeProvider = dateTimeProvider;
            _userContext = userContext;
        }

        public async Task<ApiResult<User>> CreateUser(RegisterUserDTO userDTO, CancellationToken cancellationToken)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(userDTO);
                bool isValid = Validator.TryValidateObject(userDTO, context, validationResults, true);

                if (!isValid)
                {
                    string errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                    return ApiResult<User>.Failure($"Validation failed: {errors}");
                }

                var identityId = await _authenticationService.RegisterAsync(userDTO.Email, userDTO.Password);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = userDTO.UserName,
                    Email = userDTO.Email,
                    Password = _passwordHasher.Hash(userDTO.Password),
                    Role = UserRole.Customer,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    IdentityId = identityId,
                };

                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ApiResult<User>.Success(user, "User created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResult<User>.Failure($"An error occurred while creating the user: {ex.Message}");
            }
        }

        public async Task<ApiResult<User>> GetCurrentUser(CancellationToken cancellationToken)
        {
            try
            {
                var userId = _userContext.UserId;

                if (userId == Guid.Empty)
                {
                    return ApiResult<User>.Failure("User is not authenticated.");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResult<User>.Failure($"User with ID '{userId}' not found.");
                }

                return ApiResult<User>.Success(user, "Current user retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResult<User>.Failure($"An error occurred while retrieving the current user: {ex.Message}");
            }
        }

        public async Task<ApiResult<User>> GetByEmailOrUsernameAsync(string emailOrUsername)
        {
            try
            {
                User? user = null;

                if (emailOrUsername.Contains("@"))
                {
                    user = await _userRepository.GetByEmailAsync(emailOrUsername);
                }
                else
                {
                    user = await _userRepository.GetByUsernameAsync(emailOrUsername);
                }

                if (user == null)
                {
                    return ApiResult<User>.Failure($"User with identifier '{emailOrUsername}' was not found.");
                }

                return ApiResult<User>.Success(user, "User retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResult<User>.Failure($"An error occurred while retrieving user: {ex.Message}");
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            if (users == null || !users.Any())
            {
                throw new KeyNotFoundException($"User with role '{role}' was not found.");
            }
            return users;
        }

        //public async Task<List<User>> GetActiveUsersAsync()
        //{
        //    return await _userRepository.GetActiveUsersAsync();
        //}

        public async Task<ApiResult<Page<User>>> GetUsersPaginatedAsync(
            int pageNumber,
            int pageSize,
            UserRole? roleFilter = null,
            UserStatus? statusFilter = null,
            string? searchTerm = null)
        {
            try
            {
                var usersPage = await _userRepository.GetUsersPaginatedAsync(
                    pageNumber,
                    pageSize,
                    roleFilter,
                    statusFilter,
                    searchTerm);

                return ApiResult<Page<User>>.Success(usersPage, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResult<Page<User>>.Failure($"An error occurred while retrieving users: {ex.Message}");
            }
        }


        public async Task<ApiResult<string>> UpdateUser(Guid id, UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return ApiResult<string>.Failure("User not found");
                }

                user.Username = updateUserDTO.Username;
                user.Email = updateUserDTO.Email;
                user.Role = updateUserDTO.Role;
                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return ApiResult<string>.Success($"User with ID {user.Id} was updated successfully.", "Update successful.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Failure($"An error occurred while updating user: {ex.Message}");
            }
        }


        public async Task<ApiResult<string>> InActiveUser(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResult<string>.Failure($"User with ID {userId} not found.");
                }

                user.Status = UserStatus.Inactive;
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ApiResult<string>.Success($"User with ID {user.Id} was marked as inactive.", "User inactivated.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Failure($"An error occurred while deactivating user: {ex.Message}");
            }
        }


        public async Task<ApiResult<TokenResponse>> LoginUser(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return ApiResult<TokenResponse>.Failure("Invalid email or password.");
                }

                bool isValidPassword = _passwordHasher.Verify(loginDTO.Password, user.Password);
                if (!isValidPassword)
                {
                    return ApiResult<TokenResponse>.Failure("Invalid email or password.");
                }

                var token = await _jwtProvider.GetForCredentialsAsync(
                    loginDTO.Email,
                    loginDTO.Password,
                    user.Password); 

                var response = new TokenResponse
                {
                    AccessToken = token,
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,
                    IdentityId = user.IdentityId
                };

                return ApiResult<TokenResponse>.Success(response, "Login successful.");
            }
            catch (Exception ex)
            {
                return ApiResult<TokenResponse>.Failure($"An error occurred during login: {ex.Message}");
            }
        }

                        
        public async Task<string> LoginWithFirebaseUidAsync(string uid, CancellationToken cancellationToken)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            string email = userRecord.Email;
            string name = userRecord.DisplayName;
            string identityId = userRecord.Uid;

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Username = name,
                    Password = "abcd1234",
                    Role = UserRole.Customer,
                    Status = UserStatus.Active,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    IdentityId = identityId,
                    IsProvider = true,
                };

                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return _jwtProvider.GenerateBackendJwt(user);
        }
    }
}
