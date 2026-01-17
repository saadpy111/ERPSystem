namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Website section item configuration.
    /// </summary>
    public class SectionItem
    {
        public string Id { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int Order { get; set; }
    }
}
