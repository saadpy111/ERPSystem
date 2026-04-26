using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
