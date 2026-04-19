namespace Inventory.Application.Dtos.ProductBarcodeDtos
{
    public class CreateProductBarcodeDto
    {
        public string BarcodeValue { get; set; }
        public string Type { get; set; }
        public bool IsWeighted { get; set; }
        public string? Prefix { get; set; }
        public Guid ProductId { get; set; }
    }
}