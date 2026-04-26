using MediatR;
using SharedKernel.Core.Files;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;
using System.Linq;

namespace Website.Application.Features.Brands.Queries.GetAllBrands
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, List<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetAllBrandsQueryHandler(IBrandRepository brandRepository, IFileUrlResolver fileUrlResolver)
        {
            _brandRepository = brandRepository;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<List<BrandDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllAsync();

            return brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                ImageUrl = string.IsNullOrEmpty(b.ImageUrl) ? null : _fileUrlResolver.Resolve(b.ImageUrl)
            }).ToList();
        }
    }
}
