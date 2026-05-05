namespace APBD_TEST_TEMPLATE.DTOs
{
    public class VendorProductResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal StickerPrice { get; set; }

        public ProductTypeResponseDTO ProductType { get; set; } = new();

        public ProductMakerResponseDTO ProductMaker { get; set; } = new();

        public ProductVendorOfferResponseDTO ProductVendorOffer { get; set; } = new();
    }
}
