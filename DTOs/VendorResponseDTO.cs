namespace APBD_TEST_TEMPLATE.DTOs
{
    public class VendorResponseDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public List<VendorProductResponseDTO> Products { get; set; } = new();
    }
}
