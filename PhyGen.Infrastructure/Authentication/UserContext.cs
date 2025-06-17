using Microsoft.AspNetCore.Http;
using PhyGen.Application.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Authentication
{
    internal sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserId() ??
            throw new ApplicationException("User context is unavailable");
    }
}
