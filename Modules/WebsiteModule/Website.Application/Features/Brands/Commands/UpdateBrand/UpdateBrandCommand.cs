using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Website.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommand : IRequest<UpdateBrandCommandResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
