using System.Collections.Generic;

namespace Hr.Application.DTOs
{
    public class EntityMetadataDto
    {
        public string EntityName { get; set; } = string.Empty;
        public List<OrderableFieldDto> OrderableFields { get; set; } = new List<OrderableFieldDto>();
        public List<FilterableFieldDto> FilterableFields { get; set; } = new List<FilterableFieldDto>();
        public PaginationMetadataDto Pagination { get; set; } = new PaginationMetadataDto();
    }

    public class OrderableFieldDto
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class FilterableFieldDto
    {
        public string Key { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // string, number, boolean, enum, date
        public List<string>? Values { get; set; } // For enum types
    }

    public class PaginationMetadataDto
    {
        public int DefaultPageSize { get; set; }
        public int MaxPageSize { get; set; }
    }
}