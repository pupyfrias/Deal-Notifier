using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UnlockabledPhonesController : ControllerBase
    {
        private readonly IUnlockabledPhoneServiceAsync _unlockabledPhoneServiceAsync;

        public UnlockabledPhonesController(IUnlockabledPhoneServiceAsync unlockabledPhoneServiceAsync)
        {
            _unlockabledPhoneServiceAsync = unlockabledPhoneServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<UnlockabledPhoneResponse>>>> GetAllUnlockabledPhonesAsync([FromQuery] UnlockabledPhoneFilterAndPaginationRequest request)
        {
            var pagedUnlockabledPhones = await _unlockabledPhoneServiceAsync.GetAllWithPaginationAsync<UnlockabledPhoneResponse, UnlockabledPhoneSpecification>(request);
            var response = new ApiResponse<PagedCollection<UnlockabledPhoneResponse>>(pagedUnlockabledPhones);
            return Ok(response);
        }

        // GET: api/UnlockabledPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UnlockabledPhoneResponse>>> GetUnlockabledPhone(int id)
        {
            var data = await _unlockabledPhoneServiceAsync.GetByIdProjectedAsync<UnlockabledPhoneResponse>(id);
            var response = new ApiResponse<UnlockabledPhoneResponse>(data);
            return Ok(response);
        }

        // PUT: api/UnlockabledPhones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnlockabledPhone(int id, UnlockabledPhoneUpdateRequest request)
        {
            await _unlockabledPhoneServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/UnlockabledPhones
        [HttpPost]
        public async Task<ActionResult<UnlockabledPhone>> PostUnlockabledPhone(UnlockabledPhoneCreateRequest request)
        {
            var createdUnlockabledPhone = await _unlockabledPhoneServiceAsync.CreateAsync(request);
            var response = new ApiResponse<UnlockabledPhone>(createdUnlockabledPhone);
            return CreatedAtAction("GetUnlockabledPhone", new { id = createdUnlockabledPhone.Id }, response);
        }

        // DELETE: api/UnlockabledPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnlockabledPhone(int id)
        {
            await _unlockabledPhoneServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}