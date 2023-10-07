using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Condition;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        private readonly IConditionServiceAsync _conditionServiceAsync;

        public ConditionsController(IConditionServiceAsync conditionServiceAsync)
        {
            _conditionServiceAsync = conditionServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ConditionResponse>>>> GetAllConditionsAsync([FromQuery] ConditionFilterAndPaginationRequest request)
        {
            var pagedConditions = await _conditionServiceAsync.GetAllWithPaginationAsync<ConditionResponse, ConditionSpecification>(request);
            var response = new ApiResponse<PagedCollection<ConditionResponse>>(pagedConditions);
            return Ok(response);
        }

        // GET: api/Conditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ConditionResponse>>> GetCondition(int id)
        {
            var data = await _conditionServiceAsync.GetByIdProjectedAsync<ConditionResponse>(id);
            var response = new ApiResponse<ConditionResponse>(data);
            return Ok(response);
        }

        // PUT: api/Conditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCondition(int id, ConditionUpdateRequest request)
        {
            await _conditionServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/Conditions
        [HttpPost]
        public async Task<ActionResult<Condition>> PostCondition(ConditionCreateRequest request)
        {
            var createdCondition = await _conditionServiceAsync.CreateAsync(request);
            var response = new ApiResponse<Condition>(createdCondition);
            return CreatedAtAction("GetCondition", new { id = createdCondition.Id }, response);
        }

        // DELETE: api/Conditions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCondition(int id)
        {
            await _conditionServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}