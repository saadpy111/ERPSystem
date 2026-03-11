using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.CreateCollection
{
    public class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommandRequest, CreateCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFileService _fileService;

        public CreateCollectionCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _fileService = fileService;
        }

        public async Task<CreateCollectionCommandResponse> Handle(CreateCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();

            string? imageUrl = null;
            if (request.Image != null)
            {
                imageUrl = await _fileService.SaveFileAsync(request.Image, "productcollections");
            }

            var collection = new ProductCollection
            {
                Name = request.Name,
                Slug = request.Slug ?? request.Name.ToLowerInvariant().Replace(" ", "-"),
                Description = request.Description,
                ImageUrl = imageUrl,
                IsActive = true,
                DisplayOrder = request.DisplayOrder,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await repo.AddAsync(collection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCollectionCommandResponse
            {
                Success = true,
                CollectionId = collection.Id
            };
        }
    }
}
