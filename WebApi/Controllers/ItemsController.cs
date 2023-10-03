using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Models;
using DealNotifier.Core.Application.Request;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using ILogger = Serilog.ILogger;
using Status = DealNotifier.Core.Application.Enums.Status;

namespace WebApi.Controllers
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IItemServiceAsync _itemServiceAsync;

        public ItemsController(ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            ILogger logger,
            IItemServiceAsync itemServiceAsync
            )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _itemServiceAsync = itemServiceAsync;
        }



        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<ItemResponseDto>>>> GetAllItemsAsync([FromQuery] ItemRequest request)
        {
            var data = await _itemServiceAsync.GetAllWithPaginationAsync<ItemResponseDto, ItemSpecification>(request);
            var response = new ApiResponse<PagedCollection<ItemResponseDto>>(data);
            return Ok(response);
        }


        // GET: api/Items
        [HttpGet("klk")]
        public async Task<IActionResult> Get([FromQuery] QueryParameters request)
        {
            _logger.Information("Getting all Item");
            IQueryable<Item> query = _context.Items.AsQueryable();

            if (request.Offer is not null)
            {
                if (request.Offer.Contains("all"))
                {
                    query = HandleOfferQuery(query);
                }
                else
                {
                    query = HandleTodayOfferQuery(query);
                }
            }
            else
            {
                query = HandleGeneralQuery(query, request);
            }

            //var stringQuery = query.ToQueryString();

            var items = query
                        .ProjectTo<ItemResponseDto>(_mapper.ConfigurationProvider)
                        .ToList();

            var response = new Response<List<ItemResponseDto>>(items);
            return Ok(response);
        }


        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete]
        public async Task<ActionResult> DeleteItem([FromBody] string delete)
        {
            try
            {
                List<string> idList = delete.Split(',').ToList();

                var itemList = await _context.Items.Where(x => idList.Contains(x.Id.ToString()))
                    .ToListAsync();

                if (itemList.Count == 0)
                {
                    return NotFound();
                }
                _context.Items.RemoveRange(itemList);
                var mappedItemList = _mapper.Map<List<BlackList>>(itemList);
                _context.BlackLists.AddRange(mappedItemList);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("BanKeyword")]
        public async Task<ActionResult> BanKeyword([FromBody] BanKeywordDto dto)
        {
            string query = "EXEC BAN_KEYWORD @Keyword";
            var keyword = new SqlParameter("@Keyword", dto.Keyword);
            await _context.Database.ExecuteSqlRawAsync(query, keyword);
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("{id}/cancel-notification")]
        public async Task<ActionResult> CancelNotification(Guid id)
        {
            var item =  await _context.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound(id);
            }

            item.Notify = false;
            await _context.SaveChangesAsync();

            return Ok(new {success = true});

        }


        #region Private Methods

        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

       
        private IQueryable<Item> HandleOfferQuery(IQueryable<Item> query)
        {
            return query.Where(x => x.Saving > 0)
                        .OrderByDescending(x => x.Saving)
                        .AsQueryable();

        }

        private IQueryable<Item> HandleTodayOfferQuery(IQueryable<Item> query)
        {
            return query.Where(x => x.Saving > 0 && x.LastModified >= DateTime.Now.Date)
                            .OrderByDescending(x => x.Saving)
                            .AsQueryable();

        }

        private IQueryable<Item> HandleGeneralQuery(IQueryable<Item> query, QueryParameters request)
        {
            string[]? brandList = request.Brands?.Split(",");
            string[]? storageList = request.Storages?.Split(",");
            string[]? carrierList = request.Carriers?.Split(",");
            string[]? typeList = request.Types?.Split(",");
            string[]? shopList = request.Shops?.Split(",");
            string[]? conditionList = request.Condition?.Split(",");
            decimal.TryParse(request.Max, out decimal max);
            decimal.TryParse(request.Min, out decimal min);
            bool brandExclude = request.Excludes?.Contains("brands") ?? false;

            var parameter = Expression.Parameter(typeof(Item), "item");
            var name = Expression.PropertyOrField(parameter, "Name");
            var price = Expression.PropertyOrField(parameter, "Price");
            var typeId = Expression.PropertyOrField(parameter, "TypeId");
            var shopId = Expression.PropertyOrField(parameter, "ShopId");
            var conditionId = Expression.PropertyOrField(parameter, "ConditionId");
            var body = Expression.Equal(Expression.PropertyOrField(parameter, "StatusId"), Expression.Constant((int)Status.InStock));

            /*if (request.Search is not null)
            {
                body = body.Contains(name, new string[] { request.Search }, false);
            }

            body = body.Contains(name, brandList, brandExclude);
            body = body.Contains(name, storageList, false);
            body = body.Contains(name, carrierList, false);
            body = body.Equal(typeId, typeList);
            body = body.Equal(shopId, shopList);
            body = body.Equal(conditionId, conditionList);
            body = body.LessThanOrEquall(price, max);
            body = body.GreaterThanOrEqual(price, min);
*/

            Expression<Func<Item, bool>> lambda = Expression.Lambda<Func<Item, bool>>(body, parameter);
            query = query.Where(lambda);

            return query;
        }
        #endregion Private Methods

    }
}