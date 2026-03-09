namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Hero section configuration.
    /// All text fields are now rich TextContent (text + style).
    /// BackgroundImage is now ImageContent (url + style).
    /// </summary>
    public class HeroSection
    {
        public TextContent Title { get; set; } = new();
        public TextContent Subtitle { get; set; } = new();
        public TextContent ButtonText { get; set; } = new();
        public ImageContent BackgroundImage { get; set; } = new();
    }
}
