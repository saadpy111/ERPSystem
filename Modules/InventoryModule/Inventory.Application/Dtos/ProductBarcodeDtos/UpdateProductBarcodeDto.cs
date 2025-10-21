namespace Inventory.Application.Dtos.ProductBarcodeDtos
{
    public class UpdateProductBarcodeDto
    {
        public Guid Id { get; set; }
        public string BarcodeValue { get; set; }
        public string Type { get; set; }
        public Guid ProductId { get; set; }
    }
}