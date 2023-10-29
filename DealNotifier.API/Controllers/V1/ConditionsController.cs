using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.Condition;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        private readonly IConditionService _conditionService;

        public ConditionsController(IConditionService conditionService)
        {
            _conditionService = conditionService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ConditionResponse>>>> GetAllConditionsAsync([FromQuery] ConditionFilterAndPaginationRequest request)
        {
            var pagedConditions = await _conditionService.GetAllWithPaginationAsync<ConditionResponse, ConditionSpecification>(request);
            var response = new ApiResponse<PagedCollection<ConditionResponse>>(pagedConditions);
            return Ok(response);
        }

        // GET: api/Conditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ConditionResponse>>> GetCondition(int id)
        {
            var data = await _conditionService.GetByIdProjectedAsync<ConditionResponse>(id);
            var response = new ApiResponse<ConditionResponse>(data);
            return Ok(response);
        }

        // PUT: api/Conditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCondition(int id, ConditionUpdateRequest request)
        {
            await _conditionService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/Conditions
        [HttpPost]
        public async Task<ActionResult<ConditionResponse>> PostCondition(ConditionCreateRequest request)
        {
            var createdCondition = await _conditionService.CreateAsync<ConditionCreateRequest, ConditionResponse>(request);
            var response = new ApiResponse<ConditionResponse>(createdCondition);
            return CreatedAtAction("GetCondition", new { id = createdCondition.Id }, response);
        }

        // DELETE: api/Conditions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCondition(int id)
        {
            await _conditionService.DeleteAsync(id);
            return NoContent();
        }

    }
}