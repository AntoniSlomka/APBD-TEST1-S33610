using APBD_TEST_TEMPLATE.DTOs;
using APBD_TEST_TEMPLATE.Repositories;

namespace APBD_TEST_TEMPLATE.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository _repository;

        public VendorService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<VendorResponseDTO?> GetVendor(string Code)
        {
            return _repository.GetVendorAsync(Code);
        }

    }

}
