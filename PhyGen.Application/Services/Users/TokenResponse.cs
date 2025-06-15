using PhyGen.Application.DTOs;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Users
{
    public sealed record TokenResponse
    {
        public string AccessToken { get; init; }
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; } 
        public string IdentityId { get; set; } = string.Empty;
    }
}
