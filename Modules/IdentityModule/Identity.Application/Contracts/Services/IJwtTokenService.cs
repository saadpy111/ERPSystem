using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles, IList<string> permissions, string? tenantId);
    }

}
