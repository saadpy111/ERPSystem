using MediatR;
using SharedKernel.Core.Files;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;

namespace Website.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto?>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository, IFileUrlResolver fileUrlResolver)
        {
            _brandRepository = brandRepository;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<BrandDto?> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);

            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                ImageUrl = string.IsNullOrEmpty(brand.ImageUrl) ? null : _fileUrlResolver.Resolve(brand.ImageUrl)
            };
        }
    }
}
