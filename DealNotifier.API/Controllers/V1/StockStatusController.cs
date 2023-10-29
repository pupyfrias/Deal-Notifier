using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.StockStatus;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StockStatusesController : ControllerBase
    {
        private readonly IStockStatusService _stockStatusService;

        public StockStatusesController(IStockStatusService stockStatusService)
        {
            _stockStatusService = stockStatusService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<StockStatusResponse>>>> GetAllStockStatussAsync([FromQuery] StockStatusFilterAndPaginationRequest request)
        {
            var pagedStockStatuss = await _stockStatusService.GetAllWithPaginationAsync<StockStatusResponse, StockStatusSpecification>(request);
            var response = new ApiResponse<PagedCollection<StockStatusResponse>>(pagedStockStatuss);
            return Ok(response);
        }

        // GET: api/StockStatuss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockStatusResponse>>> GetStockStatus(int id)
        {
            var data = await _stockStatusService.GetByIdProjectedAsync<StockStatusResponse>(id);
            var response = new ApiResponse<StockStatusResponse>(data);
            return Ok(response);
        }

        // PUT: api/StockStatuss/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockStatus(int id, StockStatusUpdateRequest request)
        {
            await _stockStatusService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/StockStatuss
        [HttpPost]
        public async Task<ActionResult<StockStatusResponse>> PostStockStatus(StockStatusCreateRequest request)
        {
            var createdStockStatus = await _stockStatusService.CreateAsync<StockStatusCreateRequest, StockStatusResponse>(request);
            var response = new ApiResponse<StockStatusResponse>(createdStockStatus);
            return CreatedAtAction("GetStockStatus", new { id = createdStockStatus.Id }, response);
        }

        // DELETE: api/StockStatuss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStockStatus(int id)
        {
            await _stockStatusService.DeleteAsync(id);
            return NoContent();
        }

    }
}