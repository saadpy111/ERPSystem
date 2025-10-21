using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Product>();
                var entity = await repo.GetById(request.Id, p => p.Images);
                if (entity == null)
                    return new DeleteProductCommandResponse { Success = false };

                // Delete associated images from storage
                if (entity.Images != null && entity.Images.Any())
                {
                    foreach (var img in entity.Images)
                    {
                        if (!string.IsNullOrEmpty(img.ImageUrl))
                            await _fileService.DeleteFileAsync(img.ImageUrl);
                        if (!string.IsNullOrEmpty(img.ThumbnailUrl))
                            await _fileService.DeleteFileAsync(img.ThumbnailUrl);
                    }
                }

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();

                return new DeleteProductCommandResponse { Success = true };
            }
            catch
            {
                return new DeleteProductCommandResponse { Success = false };
            }
        }
    }
}