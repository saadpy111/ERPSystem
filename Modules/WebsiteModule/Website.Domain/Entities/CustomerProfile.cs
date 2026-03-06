namespace Website.Domain.Entities
{
    /// <summary>
    /// Customer profile created automatically when a client registers.
    /// Tracks personal info, preferences, and admin notes.
    /// </summary>
    public class CustomerProfile : BaseEntity
    {
        /// <summary>
        /// Reference to the Identity user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        /// <summary>
        /// Date when the customer first registered.
        /// </summary>
        public DateTime CustomerSince { get; set; } = DateTime.UtcNow;

        public bool AllowMarketingEmails { get; set; } = true;

        public string? AdminNotes { get; set; }
    }
}
