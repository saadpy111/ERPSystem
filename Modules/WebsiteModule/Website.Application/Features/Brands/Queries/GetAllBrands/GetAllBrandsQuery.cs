using MediatR;
using System.Collections.Generic;
using Website.Application.DTOs;

namespace Website.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQuery : IRequest<List<BrandDto>>
    {
    }
}
