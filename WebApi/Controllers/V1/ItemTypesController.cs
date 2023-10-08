using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.ItemType;
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
    public class ItemTypesController : ControllerBase
    {
        private readonly IItemTypeServiceAsync _itemTypeServiceAsync;

        public ItemTypesController(IItemTypeServiceAsync itemTypeServiceAsync)
        {
            _itemTypeServiceAsync = itemTypeServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ItemTypeResponse>>>> GetAllItemTypesAsync([FromQuery] ItemTypeFilterAndPaginationRequest request)
        {
            var pagedItemTypes = await _itemTypeServiceAsync.GetAllWithPaginationAsync<ItemTypeResponse, ItemTypeSpecification>(request);
            var response = new ApiResponse<PagedCollection<ItemTypeResponse>>(pagedItemTypes);
            return Ok(response);
        }

        // GET: api/ItemTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ItemTypeResponse>>> GetItemType(int id)
        {
            var data = await _itemTypeServiceAsync.GetByIdProjectedAsync<ItemTypeResponse>(id);
            var response = new ApiResponse<ItemTypeResponse>(data);
            return Ok(response);
        }

        // PUT: api/ItemTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemType(int id, ItemTypeUpdateRequest request)
        {
            await _itemTypeServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/ItemTypes
        [HttpPost]
        public async Task<ActionResult<ItemTypeResponse>> PostItemType(ItemTypeCreateRequest request)
        {
            var createdItemType = await _itemTypeServiceAsync.CreateAsync<ItemTypeCreateRequest, ItemTypeResponse>(request);
            var response = new ApiResponse<ItemTypeResponse>(createdItemType);
            return CreatedAtAction("GetItemType", new { id = createdItemType.Id }, response);
        }

        // DELETE: api/ItemTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemType(int id)
        {
            await _itemTypeServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}