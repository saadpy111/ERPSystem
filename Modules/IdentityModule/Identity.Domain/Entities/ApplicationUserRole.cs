using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Identity.Domain.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
     
        public DateTime AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }

    }

}
