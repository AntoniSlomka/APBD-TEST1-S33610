using System.ComponentModel.DataAnnotations;

namespace APBD_TEST_TEMPLATE.DTOs
{
    public class CreateVendorDTO
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<CreateVendorProductDTO> Products { get; set; } = new();
    }
}
