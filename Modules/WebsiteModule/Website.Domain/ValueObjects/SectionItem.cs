namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Website section item configuration.
    /// Upgraded to match HeroSection capability:
    /// each section now has full text + image control.
    /// </summary>
    public class SectionItem
    {
        // ── Identity / visibility ────────────────────────────────────────
        public string Id { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int Order { get; set; }

        // ── Rich content (mirrors HeroSection structure) ─────────────────
        public TextContent Title { get; set; } = new();
        public TextContent Subtitle { get; set; } = new();
        public TextContent ButtonText { get; set; } = new();
        public ImageContent BackgroundImage { get; set; } = new();
    }
}
