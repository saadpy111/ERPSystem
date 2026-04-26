using MediatR;
using SharedKernel.Multitenancy;
using System;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdateBrandCommandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ITenantProvider _tenantProvider;

        public UpdateBrandCommandHandler(
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

        public async Task<UpdateBrandCommandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return new UpdateBrandCommandResponse { Success = false, Error = "Invalid Id" };

            if (string.IsNullOrWhiteSpace(request.Name))
                return new UpdateBrandCommandResponse { Success = false, Error = "Name is required" };

            if (request.Name.Length > 200)
                return new UpdateBrandCommandResponse { Success = false, Error = "Name must not exceed 200 characters" };

            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
                return new UpdateBrandCommandResponse { Success = false, Error = "Brand not found" };

            brand.Name = request.Name;

            if (request.Image != null)
            {
                var tenantId = _tenantProvider.GetTenantId();
                if (string.IsNullOrEmpty(tenantId))
                    return new UpdateBrandCommandResponse { Success = false, Error = "TenantId is required" };

                if (!string.IsNullOrEmpty(brand.ImageUrl))
                    await _fileService.DeleteFileAsync(brand.ImageUrl);

                var folderPath = $"websites/{tenantId}/brands/{brand.Id}";
                brand.ImageUrl = await _fileService.SaveFileAsync(request.Image, folderPath);
            }

            await _brandRepository.UpdateAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateBrandCommandResponse { Success = true };
        }
    }
}
