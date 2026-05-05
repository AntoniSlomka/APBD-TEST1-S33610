using APBD_TEST_TEMPLATE.DTOs;

namespace APBD_TEST_TEMPLATE.Repositories
{
    public interface IRepository
    {
        Task<VendorResponseDTO?> GetVendorAsync(string Code);
    }
}
