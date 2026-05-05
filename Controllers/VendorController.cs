using APBD_TEST_TEMPLATE.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TEST_TEMPLATE.Controllers
{
    [ApiController]
    [Route("api/vendors")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;            
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetVendor(string code)
        {
            var vendor = await _vendorService.GetVendor(code);
            if (vendor is null)
            {
                return NotFound($"Customer with id {code} was not found.");
            }

            return Ok(vendor);
        }

    }
}
