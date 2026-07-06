using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Queries.PlatformLogin
{
    public class PlatformLoginQueryResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        public string? Token { get; set; }

        public string? UserId { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
