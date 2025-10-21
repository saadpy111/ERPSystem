using MediatR;
using System;

namespace Inventory.Application.Features.ProductFeatures.Commands.MakeProductInactive
{
    public class MakeProductInactiveCommandRequest : IRequest<MakeProductInactiveCommandResponse>
    {
        public Guid Id { get; set; }
    }
}