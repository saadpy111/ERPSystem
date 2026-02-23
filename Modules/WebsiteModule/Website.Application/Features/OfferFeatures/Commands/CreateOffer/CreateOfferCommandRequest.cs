using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.OfferFeatures.Commands.CreateOffer
{
    public class CreateOfferCommandRequest : IRequest<CreateOfferCommandResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public int Priority { get; set; }
        public OfferScopeType ScopeType { get; set; }
        public List<Guid>? ProductIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
