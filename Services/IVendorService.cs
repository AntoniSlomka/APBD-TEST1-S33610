using APBD_TEST_TEMPLATE.DTOs;

namespace APBD_TEST_TEMPLATE.Services
{
    public interface IVendorService
    {
        Task<VendorResponseDTO?> GetVendor(string Code);
    }
}
