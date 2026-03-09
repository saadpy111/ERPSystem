namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Style properties for an image element.
    /// Controls border radius, overlay color, and overlay opacity.
    /// </summary>
    public class ImageStyle
    {
        public int BorderRadius { get; set; } = 6;
        public string OverlayColor { get; set; } = "#FFFFFF";
        public int OverlayOpacity { get; set; } = 40;
    }
}
