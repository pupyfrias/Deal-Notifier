using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.StockStatus;
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
    public class StockStatusesController : ControllerBase
    {
        private readonly IStockStatusServiceAsync _stockStatusServiceAsync;

        public StockStatusesController(IStockStatusServiceAsync stockStatusServiceAsync)
        {
            _stockStatusServiceAsync = stockStatusServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<StockStatusResponse>>>> GetAllStockStatussAsync([FromQuery] StockStatusFilterAndPaginationRequest request)
        {
            var pagedStockStatuss = await _stockStatusServiceAsync.GetAllWithPaginationAsync<StockStatusResponse, StockStatusSpecification>(request);
            var response = new ApiResponse<PagedCollection<StockStatusResponse>>(pagedStockStatuss);
            return Ok(response);
        }

        // GET: api/StockStatuss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockStatusResponse>>> GetStockStatus(int id)
        {
            var data = await _stockStatusServiceAsync.GetByIdProjectedAsync<StockStatusResponse>(id);
            var response = new ApiResponse<StockStatusResponse>(data);
            return Ok(response);
        }

        // PUT: api/StockStatuss/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockStatus(int id, StockStatusUpdateRequest request)
        {
            await _stockStatusServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/StockStatuss
        [HttpPost]
        public async Task<ActionResult<StockStatus>> PostStockStatus(StockStatusCreateRequest request)
        {
            var createdStockStatus = await _stockStatusServiceAsync.CreateAsync(request);
            var response = new ApiResponse<StockStatus>(createdStockStatus);
            return CreatedAtAction("GetStockStatus", new { id = createdStockStatus.Id }, response);
        }

        // DELETE: api/StockStatuss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStockStatus(int id)
        {
            await _stockStatusServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}