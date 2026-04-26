using MediatR;
using System;
using Website.Application.DTOs;

namespace Website.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQuery : IRequest<BrandDto?>
    {
        public Guid Id { get; set; }
    }
}
