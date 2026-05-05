using System.ComponentModel.DataAnnotations;

namespace APBD_TEST_TEMPLATE.DTOs
{
    public class CreateVendorProductDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public decimal PricePerUnit { get; set; }
    }
}
