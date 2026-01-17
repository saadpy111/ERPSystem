namespace Website.Domain.Enums
{
    /// <summary>
    /// Defines how the tenant's website is configured.
    /// </summary>
    public enum WebsiteMode
    {
        /// <summary>
        /// User provides all presentation data manually.
        /// </summary>
        Custom = 0,
        
        /// <summary>
        /// Presentation data is copied from a theme template.
        /// </summary>
        Theme = 1
    }
}
