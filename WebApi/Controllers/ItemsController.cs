using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using ILogger = Serilog.ILogger;
using Status = DealNotifier.Core.Application.Enums.Status;

namespace WebApi.Controllers
{
    [Authorize(Roles = $"{Role.SuperAdmin}")]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ItemsController(ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] queryParameters request)
        {
            _logger.Information("Getting all Item");
            IQueryable<Item> query = _context.Items.AsQueryable();

            if (request.offer is not null)
            {
                if (request.offer.Contains("all"))
                {
                    query = query.Where(x => x.Saving > 0)
                        .OrderByDescending(x => x.Saving)
                        .AsQueryable();
                }
                else
                {
                    query = query.Where(x => x.Saving > 0 && x.LastModified >= DateTime.Now.Date)
                        .OrderByDescending(x => x.Saving)
                        .AsQueryable();
                }
            }
            else
            {
                List<string>? brandList = request.brands?.Split("%2C").ToList();
                List<string>? storageList = request.storages?.Split("%2C").ToList();
                List<string>? carrierList = request.carriers?.Split("%2C").ToList();
                List<int>? typeList = request.types?.Split("%2C").Select(int.Parse).ToList();
                List<int>? shopList = request.shops?.Split("%2C").Select(int.Parse).ToList();
                List<int>? conditionList = request.condition?.Split("%2C").Select(int.Parse).ToList();
                decimal max = request.max is null ? default : decimal.Parse(request.max);
                decimal min = request.min is null ? default : decimal.Parse(request.min);
                bool brandExclude = request.excludes?.Contains("brands") ?? false;

                ParameterExpression parameter = Expression.Parameter(typeof(Item), "item");
                Expression name = Expression.PropertyOrField(parameter, "Name");
                Expression price = Expression.PropertyOrField(parameter, "Price");
                Expression typeId = Expression.PropertyOrField(parameter, "TypeId");
                Expression shoptId = Expression.PropertyOrField(parameter, "ShopId");
                Expression conditionId = Expression.PropertyOrField(parameter, "ConditionId");
                BinaryExpression body = Expression.Equal(Expression.PropertyOrField(parameter, "StatusId"), Expression.Constant((int)Status.InStock));

                if (request.search is not null)
                {
                    body = body.AddConditions(name, new List<string> { request.search }, false);
                }

                body = body.AddConditions(name, brandList, brandExclude);
                body = body.AddConditions(name, storageList, false);
                body = body.AddConditions(name, carrierList, false);
                body = body.AddConditions(typeId, typeList);
                body = body.AddConditions(shoptId, shopList);
                body = body.AddConditions(conditionId, conditionList);
                body = body.AddConditions(price, max, true);
                body = body.AddConditions(price, min, false);

                Func<Item, bool> lambda = Expression.Lambda<Func<Item, bool>>(body, parameter).Compile();
                query = query.Where(lambda).AsQueryable();
                if (request.sort_by is not null)
                {
                    var sortBy = SortBy(request.sort_by);

                    if (sortBy.Sort == "asc")
                    {
                        query = query.OrderBy(sortBy.expression).AsQueryable();
                    }
                    else
                    {
                        query = query.OrderByDescending(sortBy.expression).AsQueryable();
                    }
                }
            }

            var stringQuery = query.ToQueryString();

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
                _context.BlackLists.AddRange(_mapper.Map<List<BlackList>>(itemList));
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

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


        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        private SortBy SortBy(string str)
        {
            Expression<Func<Item, decimal>> priceAsc = i => i.Price;
            Expression<Func<Item, decimal>> priceDesc = i => i.Price;
            Expression<Func<Item, decimal>> savingDesc = i => i.Saving;
            Expression<Func<Item, decimal>> savingsPercentageDesc = i => i.SavingsPercentage;

            Dictionary<string, SortBy> dictionary = new Dictionary<string, SortBy>
            {
                { "price-low-high", new SortBy{ Sort = "asc", expression = priceAsc} },
                { "price-high-low",  new SortBy { Sort = "desc", expression = priceDesc } },
                { "saving-high-low",  new SortBy { Sort = "desc", expression = savingDesc } },
                { "saving-percent-high-low",  new SortBy { Sort = "desc", expression = savingsPercentageDesc } },
                { "default",  new SortBy { Sort = "desc", expression = priceAsc } }
            };

            if (str == null)
            {
                return dictionary["default"];
            }
            else
            {
                return dictionary.ContainsKey(str) ? dictionary[str] : dictionary["default"];
            }
        }
    }

    public class SortBy
    {
        public string Sort { get; set; }
        public Expression<Func<Item, decimal>> expression { get; set; }
    }
}