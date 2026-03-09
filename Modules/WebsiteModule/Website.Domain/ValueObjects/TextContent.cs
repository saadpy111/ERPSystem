namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// A text element with its content and visual style.
    /// Used for hero title, subtitle, button text, etc.
    /// </summary>
    public class TextContent
    {
        public string Text { get; set; } = string.Empty;
        public TextStyle Style { get; set; } = new();
    }
}
