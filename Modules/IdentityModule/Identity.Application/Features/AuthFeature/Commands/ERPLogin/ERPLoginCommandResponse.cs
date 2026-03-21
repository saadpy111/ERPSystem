using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Commands.ERPLogin
{
    public class ERPLoginCommandResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        // ── Token ────────────────────────────────────────────────────────────────
        public string? Token { get; set; }

        // ── User context (for frontend UI control only) ──────────────────────────
        // Do NOT use these for backend authorization decisions.
        // Backend security is enforced by PermissionAuthorizationHandler independently.
        public string? UserId { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
