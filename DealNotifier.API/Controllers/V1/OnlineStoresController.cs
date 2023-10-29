using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.OnlineStore;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OnlineStoresController : ControllerBase
    {
        private readonly IOnlineStoreService _onlineStoreService;

        public OnlineStoresController(IOnlineStoreService onlineStoreService)
        {
            _onlineStoreService = onlineStoreService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<OnlineStoreResponse>>>> GetAllOnlineStoresAsync([FromQuery] OnlineStoreFilterAndPaginationRequest request)
        {
            var pagedOnlineStores = await _onlineStoreService.GetAllWithPaginationAsync<OnlineStoreResponse, OnlineStoreSpecification>(request);
            var response = new ApiResponse<PagedCollection<OnlineStoreResponse>>(pagedOnlineStores);
            return Ok(response);
        }

        // GET: api/OnlineStores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OnlineStoreResponse>>> GetOnlineStore(int id)
        {
            var data = await _onlineStoreService.GetByIdProjectedAsync<OnlineStoreResponse>(id);
            var response = new ApiResponse<OnlineStoreResponse>(data);
            return Ok(response);
        }

        // PUT: api/OnlineStores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOnlineStore(int id, OnlineStoreUpdateRequest request)
        {
            await _onlineStoreService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/OnlineStores
        [HttpPost]
        public async Task<ActionResult<OnlineStoreResponse>> PostOnlineStore(OnlineStoreCreateRequest request)
        {
            var createdOnlineStore = await _onlineStoreService.CreateAsync<OnlineStoreCreateRequest, OnlineStoreResponse>(request);
            var response = new ApiResponse<OnlineStoreResponse>(createdOnlineStore);
            return CreatedAtAction("GetOnlineStore", new { id = createdOnlineStore.Id }, response);
        }

        // DELETE: api/OnlineStores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOnlineStore(int id)
        {
            await _onlineStoreService.DeleteAsync(id);
            return NoContent();
        }

    }
}