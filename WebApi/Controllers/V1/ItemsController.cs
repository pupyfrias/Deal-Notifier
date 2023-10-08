using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemServiceAsync _itemServiceAsync;

        public ItemsController(ApplicationDbContext context, IItemServiceAsync itemServiceAsync)
        {
            _context = context;
            _itemServiceAsync = itemServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ItemResponse>>>> GetAllItemsAsync([FromQuery] ItemFilterAndPaginationRequest request)
        {
            var pagedItems = await _itemServiceAsync.GetAllWithPaginationAsync<ItemResponse, ItemSpecification>(request);
            var response = new ApiResponse<PagedCollection<ItemResponse>>(pagedItems);
            return Ok(response);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ItemResponse>>> GetItem(Guid id)
        {
            var data = await _itemServiceAsync.GetByIdProjectedAsync<ItemResponse>(id);
            var response = new ApiResponse<ItemResponse>(data);
            return Ok(response);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, ItemUpdateRequest request)
        {
            await _itemServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<ItemResponse>> PostItem(ItemCreateRequest item)
        {
            var createdItem = await _itemServiceAsync.CreateAsync<ItemCreateRequest, ItemResponse>(item);
            var response = new ApiResponse<ItemResponse>(createdItem);
            return CreatedAtAction("GetItem", new { id = createdItem.Id }, response);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            await _itemServiceAsync.DeleteAsync(id);
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("{id}/cancel-notification")]
        public async Task<ActionResult> CancelNotification(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(id);
            }

            item.Notify = false;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

    }
}