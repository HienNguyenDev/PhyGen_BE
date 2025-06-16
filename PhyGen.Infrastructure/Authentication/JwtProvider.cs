using PhyGen.Application.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Authentication
{
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IPasswordHasher _passwordHasher;

        public JwtProvider(HttpClient httpClient, IPasswordHasher passwordHasher)
        {
            _httpClient = httpClient;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> GetForCredentialsAsync(string email, string password, string passwordHash)
        {
            var passwordVerified = _passwordHasher.Verify(password, passwordHash);
            if (passwordVerified == false)
            {
                throw new UnauthorizedAccessException("Invalid credentials provided.");
            }
            var request = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync("", request);

            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

            return authToken.IdToken;
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
