using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;


namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PhoneUnlockToolsController : ControllerBase
    {
        private readonly IPhoneUnlockToolService _phoneUnlockToolService;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockabledPhoneUnlockToolService;

        public PhoneUnlockToolsController(IPhoneUnlockToolService phoneUnlockToolService, IUnlockabledPhonePhoneUnlockToolService unlockabledPhoneUnlockToolService)
        {
            _phoneUnlockToolService = phoneUnlockToolService;
            _unlockabledPhoneUnlockToolService = unlockabledPhoneUnlockToolService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<PhoneUnlockToolResponse>>>> GetAllPhoneUnlockToolsAsync([FromQuery] PhoneUnlockToolFilterAndPaginationRequest request)
        {
            var pagedPhoneUnlockTools = await _phoneUnlockToolService.GetAllWithPaginationAsync<PhoneUnlockToolResponse, PhoneUnlockToolSpecification>(request);
            var response = new ApiResponse<PagedCollection<PhoneUnlockToolResponse>>(pagedPhoneUnlockTools);
            return Ok(response);
        }

        // GET: api/PhoneUnlockTools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PhoneUnlockToolResponse>>> GetPhoneUnlockTool(int id)
        {
            var data = await _phoneUnlockToolService.GetByIdProjectedAsync<PhoneUnlockToolResponse>(id);
            var response = new ApiResponse<PhoneUnlockToolResponse>(data);
            return Ok(response);
        }

        // PUT: api/PhoneUnlockTools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneUnlockTool(int id, PhoneUnlockToolUpdateRequest request)
        {
            await _phoneUnlockToolService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/PhoneUnlockTools
        [HttpPost]
        public async Task<ActionResult<PhoneUnlockToolResponse>> PostPhoneUnlockTool(PhoneUnlockToolCreateRequest request)
        {
            var createdPhoneUnlockTool = await _phoneUnlockToolService.CreateAsync<PhoneUnlockToolCreateRequest, PhoneUnlockToolResponse>(request);
            var response = new ApiResponse<PhoneUnlockToolResponse>(createdPhoneUnlockTool);
            return CreatedAtAction("GetPhoneUnlockTool", new { id = createdPhoneUnlockTool.Id }, response);
        }


        // DELETE: api/PhoneUnlockTools/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhoneUnlockTool(int id)
        {
            await _phoneUnlockToolService.DeleteAsync(id);
            return NoContent();
        }

    }
}