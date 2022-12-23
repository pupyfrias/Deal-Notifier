using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebScraping.Core.Application.Constants;
using WebScraping.Core.Application.DTOs.Item;
using WebScraping.Core.Application.Wrappers;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;
using ILogger = Serilog.ILogger;
using Type = WebScraping.Core.Application.Emuns.Type;


namespace WebApi.Controllers
{
    [Authorize(Roles = $"{Role.SuperAdmin}")]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private readonly string? _userName;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger _logger;

        public ItemsController(ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
            _logger = logger;
        }

        // GET: api/Items
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get([FromQuery] ItemRequestDTO request)
        {
            _logger.Information("Getting all Item");
      /*      var Items = _context.Items
                .ProjectTo<ItemResponseDTO>(_mapper.ConfigurationProvider)
                .Where(x => x.TypeId == (int)Type.Phone)
                .ToList();

            return Ok(Items);*/

            #region Definitions

            string? brandList = request.brands?.Replace("%2C", @"* "" or """);
            string? storageList = request.storages?.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """);
            string? carrierList = request.carriers?.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """);
            string? typeList = request.types != null ? " and (typeId= ''" + request.types.Replace("%2C", "'' or typeId= ''") + "'')" : "";
            string? shopList = request.shops != null ? " and (shopId= ''" + request.shops.Replace("%2C", "'' or shopId= ''") + "'')" : "";
            string? conditionList = request.condition != null ? " and (conditionId= " + request.condition.Replace("%2C", " or conditionId= ") + ")" : "";
            string? query = default;
            string[] excludeList = request.excludes != null ? request.excludes.Split("%2C") : new string[] { };
            List<string> parameterList = new List<string>();
            List<ItemResponseDTO> mappedItems = new List<ItemResponseDTO>();
            request.search = request.search?.Replace(" ", @"* "" and """);

            Response<List<ItemResponseDTO>>? response = default;
            List<Item>? items = default;

            #endregion Definitions

            #region Offer

            if (request.offer != null)
            {
                items = await _context.Items.FromSqlRaw($"EXEC Offer @TYPE ='{request.offer}'").ToListAsync();
                mappedItems = _mapper.Map<List<ItemResponseDTO>>(items);
                response = new Response<List<ItemResponseDTO>>(mappedItems);
                return Ok(response);
            }

            #endregion Offer

            #region Brands

            if (brandList != null)
            {
                if (excludeList.Contains("brands"))
                {
                    parameterList.Add(@$" not (""{brandList}*"") ");
                }
                else
                {
                    parameterList.Add(@$" (""{brandList}*"") ");
                }
            }

            #endregion Brands

            #region Storage

            if (storageList != null)
            {
                if (excludeList.Contains("storages"))
                {
                    parameterList.Add(@$" not (""{storageList}*"") ");
                }
                else
                {
                    parameterList.Add(@$"(""{storageList}*"") ");
                }
            }

            #endregion Storage

            #region Carrier

            if (carrierList != null)
            {
                if (excludeList.Contains("carriers"))
                {
                    parameterList.Add(@$" not (""{carrierList.Replace("%26", "&")}*"") ");
                }
                else
                {
                    parameterList.Add(@$"(""{carrierList.Replace("%26", "&")}*"") ");
                }
            }

            #endregion Carrier

            #region Search

            if (request.search != null)
            {
                parameterList.Add(@$"(""{request.search}*"") ");
            }

            int parametersCount = parameterList.Count;

            for (int i = 0; i < parametersCount; i++)
            {
                if (parameterList[i].Contains("not"))
                {
                    var data = parameterList[i];
                    parameterList.RemoveAt(i);
                    parameterList.Add(data);
                }
            }

            #endregion Search

            #region Sort By

            request.sort_by = SortBy(request.sort_by);

            #endregion Sort By

            #region Price

            if (request.min != null)
            {
                request.min = $" and price > {request.min} ";
            }
            else { request.min = ""; }

            if (request.max != null)
            {
                request.max = $" and price < {request.max} ";
            }
            else { request.max = ""; }

            #endregion Price

            if (parameterList.Count > 0)
            {
                if (parameterList[0].Contains("not"))
                {
                    parameterList[0] = parameterList[0].Replace("not", "");
                    query = $"EXEC Filter @Data = 'and not contains (NAME, ''{string.Join(" and ", parameterList)}'') ', @Order = '{request.sort_by}', @Min ='{request.min}', @Max ='{request.max}' ,@Types ='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
                }
                else
                {
                    query = $"EXEC Filter @Data = 'and contains (NAME, ''{string.Join(" and ", parameterList)}'') ', @Order = '{request.sort_by}', @Min ='{request.min}', @Max ='{request.max}' ,@Types ='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
                }
            }
            else
            {
                query = $"EXEC Filter @Data = '', @Order = '{request.sort_by}', @Min ='{request.min}', @Max ='{request.max}',@types='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
            }

            items = await _context.Items.FromSqlRaw(query).ToListAsync();
            mappedItems = _mapper.Map<List<ItemResponseDTO>> (items);
            response = new Response<List<ItemResponseDTO>>(mappedItems);
            return Ok(response);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetItem(int id)
        {
            var item = await Task.Run(() => _context.Items.FromSqlRaw($"EXEC SP_GET_ONE @ID={id}").AsEnumerable().FirstOrDefault());

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete]
        public async Task<ActionResult> DeleteItem([FromQuery]string delete)
        {
            try
            {
                List<string> idList = delete.Split(',').ToList();
                
                var itemList = await _context.Items.Where(x=> idList.Contains(x.Id.ToString()))
                    .ToListAsync();
                
                if (itemList.Count == 0)
                {
                    NotFound();
                }
                _context.Items.RemoveRange(itemList);
                await _context.SaveChangesAsync();
                
                return NoContent() ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        private string SortBy(string str)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "price-low-high", " price asc"},
                { "price-high-low", "price desc"},
                { "saving-high-low", "saving desc"},
                { "saving-percent-high-low", "SavingsPercentage desc"},
                { "default", "id desc"}
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
}