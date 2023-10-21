using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Persistence.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IItemService _itemService;

        public ItemsController(ApplicationDbContext context, IItemService itemService)
        {
            _dbContext = context;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ItemResponse>>>> GetAllItemsAsync([FromQuery] ItemFilterAndPaginationRequest request)
        {
            var pagedItems = await _itemService.GetAllWithPaginationAsync<ItemResponse, ItemSpecification>(request);
            var response = new ApiResponse<PagedCollection<ItemResponse>>(pagedItems);
            return Ok(response);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ItemResponse>>> GetItem(Guid id)
        {
            var data = await _itemService.GetByPublicIdProjectedAsync<ItemResponse>(id);
            var response = new ApiResponse<ItemResponse?>(data);
            return Ok(response);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, ItemUpdateRequest request)
        {
            await _itemService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<ItemResponse>> PostItem(ItemCreateRequest item)
        {
            var createdItem = await _itemService.CreateAsync<ItemCreateRequest, ItemResponse>(item);
            var response = new ApiResponse<ItemResponse>(createdItem);
            return CreatedAtAction("GetItem", new { id = createdItem.Id }, response);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            await _itemService.DeleteAsync(id);
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("{id}/cancel-notification")]
        public async Task<ActionResult> CancelNotification(Guid id)
        {
            var item = await _dbContext.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(id);
            }

            item.Notify = false;
            await _dbContext.SaveChangesAsync();

            return Ok(new { success = true });
        }

    }
}