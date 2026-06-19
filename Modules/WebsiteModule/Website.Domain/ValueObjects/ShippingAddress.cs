namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Value object for shipping address.
    /// Immutable record type for use in Order entity.
    /// </summary>
    public record ShippingAddress
    {
        public string RecipientName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string Street { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public string ZipCode { get; init; } = string.Empty;

        public ShippingAddress() { }

        public ShippingAddress(string recipientName, string phone, string street, string city, string state, string country, string zipCode)
        {
            RecipientName = recipientName;
            Phone = phone;
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }
    }
}
