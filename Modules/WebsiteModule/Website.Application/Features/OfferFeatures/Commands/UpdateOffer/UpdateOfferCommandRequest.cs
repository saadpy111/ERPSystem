using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.OfferFeatures.Commands.UpdateOffer
{
    public class UpdateOfferCommandRequest : IRequest<UpdateOfferCommandResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DiscountType? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
