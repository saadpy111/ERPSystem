using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Commands.AddProductAttributeValues
{
    public class AddProductAttributeValuesCommandHandler : IRequestHandler<AddProductAttributeValuesCommandRequest, AddProductAttributeValuesCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductAttributeValuesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddProductAttributeValuesCommandResponse> Handle(AddProductAttributeValuesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.Repositories<Product>().GetById(request.ProductId);
                if (product == null) return new AddProductAttributeValuesCommandResponse { Success = false };

                foreach (var attr in request.AttributeValues)
                {
                    var attrValue = new ProductAttributeValue
                    {
                        ProductId = request.ProductId,
                        AttributeId = attr.AttributeId,
                        Value = attr.Value,
                         CreatedAt = DateTime.UtcNow
                          
                         
                    };
                    await _unitOfWork.Repositories<ProductAttributeValue>().Add(attrValue);
                }
                await _unitOfWork.CompleteAsync();

                return new AddProductAttributeValuesCommandResponse { Success = true };
            }
            catch
            {
                return new AddProductAttributeValuesCommandResponse { Success = false };
            }
        }
    }
}