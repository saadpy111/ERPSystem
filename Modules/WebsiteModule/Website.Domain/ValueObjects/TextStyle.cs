using Website.Domain.Enums;

namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Style properties for a text element.
    /// Controls visual rendering: size, weight, color, alignment, and spacing.
    /// </summary>
    public class TextStyle
    {
        public int FontSize { get; set; } = 16;
        public FontWeight FontWeight { get; set; } = FontWeight.Normal;
        public string Color { get; set; } = "#000000";
        public TextAlign Alignment { get; set; } = TextAlign.Left;
        public int HorizontalSpacing { get; set; } = 0;
        public int VerticalSpacing { get; set; } = 0;

        /// <summary>
        /// Top margin in pixels. Defaults to 0 for backward compatibility.
        /// </summary>
        public int? MarginTop { get; set; }
    }
}
