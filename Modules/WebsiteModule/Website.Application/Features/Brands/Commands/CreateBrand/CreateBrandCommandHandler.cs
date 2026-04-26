using MediatR;
using SharedKernel.Multitenancy;
using System;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;

namespace Website.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreateBrandCommandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ITenantProvider _tenantProvider;

        public CreateBrandCommandHandler(
            IBrandRepository brandRepository,
            IWebsiteUnitOfWork unitOfWork,
            IFileService fileService,
            ITenantProvider tenantProvider)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _tenantProvider = tenantProvider;
        }

        public async Task<CreateBrandCommandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new CreateBrandCommandResponse { Success = false, Error = "Name is required" };

            if (request.Name.Length > 200)
                return new CreateBrandCommandResponse { Success = false, Error = "Name must not exceed 200 characters" };

            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return new CreateBrandCommandResponse { Success = false, Error = "TenantId is required" };

            var brand = new Brand
            {
                Name = request.Name,
                TenantId = tenantId
            };

            await _brandRepository.AddAsync(brand);

            if (request.Image != null)
            {
                var folderPath = $"websites/{tenantId}/brands/{brand.Id}";
                brand.ImageUrl = await _fileService.SaveFileAsync(request.Image, folderPath);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateBrandCommandResponse { Success = true };
        }
    }
}
