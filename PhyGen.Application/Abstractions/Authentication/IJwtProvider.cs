using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<string> GetForCredentialsAsync(string email, string password, string passwordHash);
        string GenerateBackendJwt(User user);
    }
}
