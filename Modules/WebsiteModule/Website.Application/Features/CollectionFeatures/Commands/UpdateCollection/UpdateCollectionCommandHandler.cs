using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.UpdateCollection
{
    public class UpdateCollectionCommandHandler : IRequestHandler<UpdateCollectionCommandRequest, UpdateCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCollectionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCollectionCommandResponse> Handle(UpdateCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(request.Id);

            if (collection == null)
            {
                return new UpdateCollectionCommandResponse
                {
                    Success = false,
                    Message = "Collection not found."
                };
            }

            // Update only provided fields
            if (request.Name != null) collection.Name = request.Name;
            if (request.Slug != null) collection.Slug = request.Slug;
            if (request.Description != null) collection.Description = request.Description;
            if (request.ImageUrl != null) collection.ImageUrl = request.ImageUrl;
            if (request.IsActive.HasValue) collection.IsActive = request.IsActive.Value;
            if (request.DisplayOrder.HasValue) collection.DisplayOrder = request.DisplayOrder.Value;
            collection.UpdatedAt = DateTime.UtcNow;

            repo.Update(collection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateCollectionCommandResponse { Success = true };
        }
    }
}
