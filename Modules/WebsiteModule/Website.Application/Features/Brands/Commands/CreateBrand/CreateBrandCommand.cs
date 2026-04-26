using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Website.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<CreateBrandCommandResponse>
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
