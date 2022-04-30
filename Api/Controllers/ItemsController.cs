using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly contextItem _context;

        public ItemsController(contextItem context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<Item>> GetItem(string brands, string storages, string search, string max, string min,
            string order_by, string offer, string types, string condition, string carriers, string excludes, string shops)
        {
            string listBrands = brands != null ? brands.Replace("%2C", @"* "" or """) : null;
            string listStorages = storages != null ? storages.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """) : null;
            string listCarriers = carriers != null ? carriers.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """) : null;
            string listTypes = types != null ? " and (type= ''" + types.Replace("%2C", "'' or type= ''") + "'')" : "";
            string listShops = shops != null ? " and (shop= ''" + shops.Replace("%2C", "'' or shop= ''") + "'')" : "";
            string listCondition = condition != null ? " and (condition= " + condition.Replace("%2C", " or condition= ") + ")" : "";
            string[] listExcludes = { };
            listExcludes = excludes != null ? excludes.Split("%2C") : listExcludes;

            List<string> parameters = new List<string>();
            string query;
            search = search != null ? search.Replace(" ", @"* "" and """) : null;

            //OFFER
            if (offer != null)
            {
                return await _context.Item.FromSqlRaw("EXEC Offer").ToListAsync();
            }

            //BRANDS
            if (listBrands != null)
            {
                if (listExcludes.Contains("brands"))
                {
                    parameters.Add(@$" not (""{listBrands}*"") ");
                }
                else
                {
                    parameters.Add(@$" (""{listBrands}*"") ");
                }

            }

            //STORAGES
            if (listStorages != null)
            {
                if (listExcludes.Contains("storages"))
                {
                    parameters.Add(@$" not (""{listStorages}*"") ");
                }
                else
                {
                    parameters.Add(@$"(""{listStorages}*"") ");
                }
            }

            //CARRIERS
            if (listCarriers != null)
            {
                if (listExcludes.Contains("carriers"))
                {
                    parameters.Add(@$" not (""{listCarriers.Replace("%26", "&")}*"") ");
                }
                else
                {
                    parameters.Add(@$"(""{listCarriers.Replace("%26", "&")}*"") ");
                }
            }

            //SEARCH
            if (search != null)
            {
                parameters.Add(@$"(""{search}*"") ");
            }

            int parametersCount = parameters.Count;

            for (int i = 0; i < parametersCount; i++)
            {
                if (parameters[i].Contains("not"))
                {
                    var data = parameters[i];
                    parameters.RemoveAt(i);
                    parameters.Add(data);
                }
            }

            //order by
            if (order_by != null)
            {
                if (order_by == "price-low-high") { order_by = " price asc"; }
                else if (order_by == "price-high-low") { order_by = "price desc"; }
                else if (order_by == "saving-high-low") { order_by = " saving desc"; }
                else if (order_by == "saving-percent-high-low") { order_by = "saving_percent desc"; }
            }
            else { order_by = "id desc"; }


            //price
            if (min != null)
            {
                min = $" and price > {min} ";
            }
            else { min = ""; }

            if (max != null)
            {
                max = $" and price < {max} ";
            }
            else { max = ""; }



            // EXEC Filter @Data = 'AND CONTAINS (NAME,''SAMSUNG OR IPHONE'')', @Page = 1, @Order = 'PRICE ASC', @Min = '', @Max = ''
            if (parameters.Count > 0)
            {
                if (parameters[0].Contains("not"))
                {
                    parameters[0] = parameters[0].Replace("not", "");
                    query = $"EXEC Filter @Data = 'and not contains (NAME, ''{String.Join(" and ", parameters) }'') ', @Order = '{order_by}', @Min ='{min}', @Max ='{max}' ,@Types ='{listTypes}',@Shops ='{listShops}', @Condition='{listCondition}'";
                }
                else
                {
                    query = $"EXEC Filter @Data = 'and contains (NAME, ''{ String.Join(" and ", parameters) }'') ', @Order = '{order_by}', @Min ='{min}', @Max ='{max}' ,@Types ='{listTypes}',@Shops ='{listShops}', @Condition='{listCondition}'";
                }

            }
            else
            {
                query = $"EXEC Filter @Data = '', @Order = '{order_by}', @Min ='{min}', @Max ='{max}',@types='{listTypes}',@Shops ='{listShops}', @Condition='{listCondition}'";
            }

            return await _context.Item.FromSqlRaw(query).ToListAsync();

        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetItem(int id)
        {
            var item = await Task.Run(() => _context.Item.FromSqlRaw($"EXEC SP_GET_ONE @ID={id}").AsEnumerable().FirstOrDefault());

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
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.id)
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
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await Task.Run(() => _context.Item.FromSqlRaw($"EXEC SP_GET_ONE @ID={id}").AsEnumerable().FirstOrDefault());

            if (item == null)
            {
                return NotFound();
            }

            string query = $"EXEC SP_DELETE @LINK= '{item.link}'";
            _context.Database.ExecuteSqlRaw(query);


            return item;

        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.id == id);
        }
    }
}
