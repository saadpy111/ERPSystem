namespace Website.Application.Features.OfferFeatures.Commands.CreateOffer
{
    public class CreateOfferCommandResponse
    {
        public bool Success { get; set; }
        public Guid? OfferId { get; set; }
        public string? Message { get; set; }
    }
}
