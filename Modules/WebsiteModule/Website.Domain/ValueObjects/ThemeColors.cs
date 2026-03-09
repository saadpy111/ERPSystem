namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Theme color palette plus global font family.
    /// </summary>
    public class ThemeColors
    {
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Global font family used across the entire website.
        /// Default: "Neo Sans Arabic"
        /// </summary>
        public string FontFamily { get; set; } = "Neo Sans Arabic";
    }
}
