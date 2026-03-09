namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// An image element with its URL and visual style.
    /// Used for hero background image, etc.
    /// </summary>
    public class ImageContent
    {
        public string Url { get; set; } = string.Empty;
        public ImageStyle Style { get; set; } = new();
    }
}
