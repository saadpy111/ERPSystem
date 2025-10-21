using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.DeleteProductBarcode
{
    public class DeleteProductBarcodeCommandHandler : IRequestHandler<DeleteProductBarcodeCommandRequest, DeleteProductBarcodeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductBarcodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductBarcodeCommandResponse> Handle(DeleteProductBarcodeCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<ProductBarcode>();
                var entity = await repo.GetById(request.Id);
                if (entity == null)
                    return new DeleteProductBarcodeCommandResponse { Success = false };

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();
                return new DeleteProductBarcodeCommandResponse { Success = true };
            }
            catch
            {
                return new DeleteProductBarcodeCommandResponse { Success = false };

            }

        }
    }
}