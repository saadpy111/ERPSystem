using MediatR;
using System;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryRequest : IRequest<GetProductByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}