using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Authentication
{
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public JwtProvider(HttpClient httpClient, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        //public async Task<string> GetForCredentialsAsync(string email, string password, string passwordHash)
        //{
        //    var passwordVerified = _passwordHasher.Verify(password, passwordHash);
        //    if (passwordVerified == false)
        //    {
        //        throw new UnauthorizedAccessException("Invalid credentials provided.");
        //    }
        //    var request = new
        //    {
        //        email,
        //        password,
        //        returnSecureToken = true
        //    };

        //    var response = await _httpClient.PostAsJsonAsync("", request);

        //    var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

        //    return authToken.IdToken;
        //}

        public async Task<string> GetForCredentialsAsync(string email, string password, string passwordHash)
        {
            var passwordVerified = _passwordHasher.Verify(password, passwordHash);
            if (!passwordVerified)
            {
                throw new UnauthorizedAccessException("Invalid credentials provided.");
            }

            var role = "Customer"; 
            var identityId = "";    

            return (GenerateBackendJwt(email, role, identityId));
        }

        public async Task<string> GetFromGoogleIdTokenAsync(string firebaseIdToken)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseIdToken);
            var email = decodedToken.Claims["email"].ToString();
            var identityId = decodedToken.Uid;
            var role = "Customer"; 

            return GenerateBackendJwt(email, role, identityId);
        }

        private string GenerateBackendJwt(string email, string role, string identityId)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
            };

            if (!string.IsNullOrWhiteSpace(identityId))
            {
                claims.Add(new Claim("IdentityId", identityId));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string GenerateBackendJwt(User user)
        {
            return GenerateBackendJwt(user.Email, user.Role.ToString(), user.IdentityId);
        }

        public class AuthToken
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; }

            [JsonPropertyName("localId")]
            public string LocalId { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }

            [JsonPropertyName("idToken")]
            public string IdToken { get; set; }

            [JsonPropertyName("registered")]
            public bool Registered { get; set; }

            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }

            [JsonPropertyName("expiresIn")]
            public string ExpiresIn { get; set; }
        } 
    }
}
