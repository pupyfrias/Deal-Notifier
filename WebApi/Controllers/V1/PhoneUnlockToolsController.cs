using DealNotifier.Core.Application.ViewModels.Common;
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

        public PhoneUnlockToolsController(IPhoneUnlockToolServiceAsync phoneUnlockToolServiceAsync)
        {
            _phoneUnlockToolServiceAsync = phoneUnlockToolServiceAsync;
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
        public async Task<ActionResult<PhoneUnlockTool>> PostPhoneUnlockTool(PhoneUnlockToolCreateRequest request)
        {
            var createdPhoneUnlockTool = await _phoneUnlockToolServiceAsync.CreateAsync(request);
            var response = new ApiResponse<PhoneUnlockTool>(createdPhoneUnlockTool);
            return CreatedAtAction("GetPhoneUnlockTool", new { id = createdPhoneUnlockTool.Id }, response);
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