using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.UnlockProbability;
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
    public class UnlockProbabilitiesController : ControllerBase
    {
        private readonly IUnlockProbabilityServiceAsync _unlockUnlockProbabilityServiceAsync;

        public UnlockProbabilitiesController(IUnlockProbabilityServiceAsync unlockUnlockProbabilityServiceAsync)
        {
            _unlockUnlockProbabilityServiceAsync = unlockUnlockProbabilityServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<UnlockProbabilityResponse>>>> GetAllUnlockProbabilitiesAsync([FromQuery] UnlockProbabilityFilterAndPaginationRequest request)
        {
            var pagedUnlockProbabilities = await _unlockUnlockProbabilityServiceAsync.GetAllWithPaginationAsync<UnlockProbabilityResponse, UnlockProbabilitySpecification>(request);
            var response = new ApiResponse<PagedCollection<UnlockProbabilityResponse>>(pagedUnlockProbabilities);
            return Ok(response);
        }

        // GET: api/UnlockProbabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UnlockProbabilityResponse>>> GetUnlockProbability(int id)
        {
            var data = await _unlockUnlockProbabilityServiceAsync.GetByIdProjectedAsync<UnlockProbabilityResponse>(id);
            var response = new ApiResponse<UnlockProbabilityResponse>(data);
            return Ok(response);
        }

        // PUT: api/UnlockProbabilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnlockProbability(int id, UnlockProbabilityUpdateRequest request)
        {
            await _unlockUnlockProbabilityServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/UnlockProbabilities
        [HttpPost]
        public async Task<ActionResult<UnlockProbabilityResponse>> PostUnlockProbability(UnlockProbabilityCreateRequest request)
        {
            var createdUnlockProbability = await _unlockUnlockProbabilityServiceAsync.CreateAsync<UnlockProbabilityCreateRequest, UnlockProbabilityResponse>(request);
            var response = new ApiResponse<UnlockProbabilityResponse>(createdUnlockProbability);
            return CreatedAtAction("GetUnlockProbability", new { id = createdUnlockProbability.Id }, response);
        }

        // DELETE: api/UnlockProbabilities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnlockProbability(int id)
        {
            await _unlockUnlockProbabilityServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}