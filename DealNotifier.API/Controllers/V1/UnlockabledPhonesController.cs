using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.UnlockabledPhone;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UnlockabledPhonesController : ControllerBase
    {
        private readonly IUnlockabledPhoneService _unlockabledPhoneService;

        public UnlockabledPhonesController(IUnlockabledPhoneService unlockabledPhoneService)
        {
            _unlockabledPhoneService = unlockabledPhoneService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<UnlockabledPhoneResponse>>>> GetAllUnlockabledPhonesAsync([FromQuery] UnlockabledPhoneFilterAndPaginationRequest request)
        {
            var pagedUnlockabledPhones = await _unlockabledPhoneService.GetAllWithPaginationAsync<UnlockabledPhoneResponse, UnlockabledPhoneSpecification>(request);
            var response = new ApiResponse<PagedCollection<UnlockabledPhoneResponse>>(pagedUnlockabledPhones);
            return Ok(response);
        }

        // GET: api/UnlockabledPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UnlockabledPhoneResponse>>> GetUnlockabledPhone(int id)
        {
            var data = await _unlockabledPhoneService.GetByIdProjectedAsync<UnlockabledPhoneResponse>(id);
            var response = new ApiResponse<UnlockabledPhoneResponse>(data);
            return Ok(response);
        }

        // PUT: api/UnlockabledPhones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnlockabledPhone(int id, UnlockabledPhoneUpdateRequest request)
        {
            await _unlockabledPhoneService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/UnlockabledPhones
        [HttpPost]
        public async Task<ActionResult<UnlockabledPhoneResponse>> PostUnlockabledPhone(UnlockabledPhoneCreateRequest request)
        {
            var createdUnlockabledPhone = await _unlockabledPhoneService.CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(request);
            var response = new ApiResponse<UnlockabledPhoneResponse>(createdUnlockabledPhone);
            return CreatedAtAction("GetUnlockabledPhone", new { id = createdUnlockabledPhone.Id }, response);
        }

        // DELETE: api/UnlockabledPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnlockabledPhone(int id)
        {
            await _unlockabledPhoneService.DeleteAsync(id);
            return NoContent();
        }

    }
}