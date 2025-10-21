using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.DeleteProductAttribute
{
    public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommandRequest, DeleteProductAttributeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductAttributeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductAttributeCommandResponse> Handle(DeleteProductAttributeCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repositories<ProductAttribute>();
            var entity = await repo.GetById(request.Id);

            if (entity == null) return new DeleteProductAttributeCommandResponse()
            {
                Success = false
            };

            repo.Remove(entity);
            await _unitOfWork.CompleteAsync();
            return new DeleteProductAttributeCommandResponse()
            {
                Success = true
            };
        }
    }
}