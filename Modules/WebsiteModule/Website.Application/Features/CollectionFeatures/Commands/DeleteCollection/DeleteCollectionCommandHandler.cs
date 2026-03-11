using MediatR;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.DeleteCollection
{
    public class DeleteCollectionCommandHandler : IRequestHandler<DeleteCollectionCommandRequest, DeleteCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteCollectionCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<DeleteCollectionCommandResponse> Handle(DeleteCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(request.Id);

            if (collection == null)
            {
                return new DeleteCollectionCommandResponse
                {
                    Success = false,
                    Message = "Collection not found."
                };
            }

            if (!string.IsNullOrEmpty(collection.ImageUrl))
            {
                await _fileService.DeleteFileAsync(collection.ImageUrl);
            }

            repo.Remove(collection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteCollectionCommandResponse { Success = true };
        }
    }
}
