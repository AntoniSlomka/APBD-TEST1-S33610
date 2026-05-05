using APBD_TEST_TEMPLATE.DTOs;
using APBD_TEST_TEMPLATE.Excpetions;
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

        [HttpPost]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDTO request)
        {
            try
            {
                await _vendorService.CreateVendor(request);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetVendor), new { request.Code }, null);
        }

    }
}
