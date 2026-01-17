using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.CreateCollection
{
    public class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommandRequest, CreateCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public CreateCollectionCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<CreateCollectionCommandResponse> Handle(CreateCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();

            var collection = new ProductCollection
            {
                Name = request.Name,
                Slug = request.Slug ?? request.Name.ToLowerInvariant().Replace(" ", "-"),
                Description = request.Description,
                ImageUrl = request.ImageUrl,
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
