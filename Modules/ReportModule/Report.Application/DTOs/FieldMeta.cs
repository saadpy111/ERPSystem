namespace Report.Application.DTOs
{
    public class FieldMeta
    {
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? DataType { get; set; }
        public int? Width { get; set; }
        public bool IsVisible { get; set; }
    }
}