using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.OnlineStore;
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
    public class OnlineStoresController : ControllerBase
    {
        private readonly IOnlineStoreServiceAsync _onlineStoreServiceAsync;

        public OnlineStoresController(IOnlineStoreServiceAsync onlineStoreServiceAsync)
        {
            _onlineStoreServiceAsync = onlineStoreServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<OnlineStoreResponse>>>> GetAllOnlineStoresAsync([FromQuery] OnlineStoreFilterAndPaginationRequest request)
        {
            var pagedOnlineStores = await _onlineStoreServiceAsync.GetAllWithPaginationAsync<OnlineStoreResponse, OnlineStoreSpecification>(request);
            var response = new ApiResponse<PagedCollection<OnlineStoreResponse>>(pagedOnlineStores);
            return Ok(response);
        }

        // GET: api/OnlineStores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OnlineStoreResponse>>> GetOnlineStore(int id)
        {
            var data = await _onlineStoreServiceAsync.GetByIdProjectedAsync<OnlineStoreResponse>(id);
            var response = new ApiResponse<OnlineStoreResponse>(data);
            return Ok(response);
        }

        // PUT: api/OnlineStores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOnlineStore(int id, OnlineStoreUpdateRequest request)
        {
            await _onlineStoreServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/OnlineStores
        [HttpPost]
        public async Task<ActionResult<OnlineStore>> PostOnlineStore(OnlineStoreCreateRequest request)
        {
            var createdOnlineStore = await _onlineStoreServiceAsync.CreateAsync(request);
            var response = new ApiResponse<OnlineStore>(createdOnlineStore);
            return CreatedAtAction("GetOnlineStore", new { id = createdOnlineStore.Id }, response);
        }

        // DELETE: api/OnlineStores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOnlineStore(int id)
        {
            await _onlineStoreServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}