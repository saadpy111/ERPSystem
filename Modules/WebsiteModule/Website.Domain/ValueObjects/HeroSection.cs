namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Hero section configuration.
    /// </summary>
    public class HeroSection
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public string BackgroundImage { get; set; } = string.Empty;
    }
}
