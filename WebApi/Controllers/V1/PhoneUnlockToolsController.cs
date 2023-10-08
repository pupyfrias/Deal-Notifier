using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
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
    public class PhoneUnlockToolsController : ControllerBase
    {
        private readonly IPhoneUnlockToolServiceAsync _phoneUnlockToolServiceAsync;
        private readonly IUnlockabledPhonePhoneUnlockToolServiceAsync _unlockabledPhoneUnlockToolServiceAsync;

        public PhoneUnlockToolsController(IPhoneUnlockToolServiceAsync phoneUnlockToolServiceAsync, IUnlockabledPhonePhoneUnlockToolServiceAsync unlockabledPhoneUnlockToolServiceAsync)
        {
            _phoneUnlockToolServiceAsync = phoneUnlockToolServiceAsync;
            _unlockabledPhoneUnlockToolServiceAsync = unlockabledPhoneUnlockToolServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<PhoneUnlockToolResponse>>>> GetAllPhoneUnlockToolsAsync([FromQuery] PhoneUnlockToolFilterAndPaginationRequest request)
        {
            var pagedPhoneUnlockTools = await _phoneUnlockToolServiceAsync.GetAllWithPaginationAsync<PhoneUnlockToolResponse, PhoneUnlockToolSpecification>(request);
            var response = new ApiResponse<PagedCollection<PhoneUnlockToolResponse>>(pagedPhoneUnlockTools);
            return Ok(response);
        }

        // GET: api/PhoneUnlockTools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PhoneUnlockToolResponse>>> GetPhoneUnlockTool(int id)
        {
            var data = await _phoneUnlockToolServiceAsync.GetByIdProjectedAsync<PhoneUnlockToolResponse>(id);
            var response = new ApiResponse<PhoneUnlockToolResponse>(data);
            return Ok(response);
        }

        // PUT: api/PhoneUnlockTools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneUnlockTool(int id, PhoneUnlockToolUpdateRequest request)
        {
            await _phoneUnlockToolServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/PhoneUnlockTools
        [HttpPost]
        public async Task<ActionResult<PhoneUnlockToolResponse>> PostPhoneUnlockTool(PhoneUnlockToolCreateRequest request)
        {
            var createdPhoneUnlockTool = await _phoneUnlockToolServiceAsync.CreateAsync<PhoneUnlockToolCreateRequest, PhoneUnlockToolResponse>(request);
            var response = new ApiResponse<PhoneUnlockToolResponse>(createdPhoneUnlockTool);
            return CreatedAtAction("GetPhoneUnlockTool", new { id = createdPhoneUnlockTool.Id }, response);
        }

        // POST: api/PhoneUnlockTools
        [HttpPost("{PhoneUnlockToolId}/UnlockabledPhone")]
        public async Task<ActionResult<PhoneUnlockTool>> PostPhoneUnlockToolUnlockablePhone(int PhoneUnlockToolId, PhoneUnlockToolUnlockablePhoneCreateRequest request)
        {
            await _unlockabledPhoneUnlockToolServiceAsync.CreateRangeAsync(PhoneUnlockToolId, request);
            return NoContent();
        }


        // DELETE: api/PhoneUnlockTools/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhoneUnlockTool(int id)
        {
            await _phoneUnlockToolServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}