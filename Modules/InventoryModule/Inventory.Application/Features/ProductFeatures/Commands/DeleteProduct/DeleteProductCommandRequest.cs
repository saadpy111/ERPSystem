using MediatR;
using System;

namespace Inventory.Application.Features.ProductFeatures.Commands.DeleteProduct
{
    public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
    {
        public Guid Id { get; set; }
    }
}