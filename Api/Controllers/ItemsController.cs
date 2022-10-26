using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScraping.Core.Domain.Entities;
using WebScraping.Intrastructure.Persistence.DbContexts;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<List<Item>> GetItem(string brands, string storages, string search, string max, string min,
            string sort_by, string offer, string types, string condition, string carriers, string excludes, string shops)
        {
            #region Definitions
            string brandList = brands?.Replace("%2C", @"* "" or """);
            string storageList = storages?.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """);
            string carrierList = carriers?.Replace("%2C", @"* "" or """).Replace("%2B", " ").Replace("+", @"* "" and """);
            string typeList = types != null ? " and (typeId= ''" + types.Replace("%2C", "'' or typeId= ''") + "'')" : "";
            string shopList = shops != null ? " and (shopId= ''" + shops.Replace("%2C", "'' or shopId= ''") + "'')" : "";
            string conditionList = condition != null ? " and (conditionId= " + condition.Replace("%2C", " or conditionId= ") + ")" : "";
            string query = default;
            string[] excludeList = excludes != null ? excludes.Split("%2C") : new string[] {};
            List<string> parameterList = new List<string>();
            search = search?.Replace(" ", @"* "" and """);
            #endregion Definitions

            #region  Offer
            if (offer != null)
            {
                return await _context.Items.FromSqlRaw($"EXEC Offer @TYPE ='{offer}'").ToListAsync();
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
            if (search != null)
            {
                parameterList.Add(@$"(""{search}*"") ");
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
            sort_by = SortBy(sort_by);
            #endregion Sort By

            #region Price
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
            #endregion Price
            
            if (parameterList.Count > 0)
            {
                if (parameterList[0].Contains("not"))
                {
                    parameterList[0] = parameterList[0].Replace("not", "");
                    query = $"EXEC Filter @Data = 'and not contains (NAME, ''{String.Join(" and ", parameterList)}'') ', @Order = '{sort_by}', @Min ='{min}', @Max ='{max}' ,@Types ='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
                }
                else
                {
                    query = $"EXEC Filter @Data = 'and contains (NAME, ''{String.Join(" and ", parameterList)}'') ', @Order = '{sort_by}', @Min ='{min}', @Max ='{max}' ,@Types ='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
                }

            }
            else
            {
                query = $"EXEC Filter @Data = '', @Order = '{sort_by}', @Min ='{min}', @Max ='{max}',@types='{typeList}',@Shops ='{shopList}', @Condition='{conditionList}'";
            }

            return await _context.Items.FromSqlRaw(query).ToListAsync();

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
        public async Task<IActionResult> PutItem(int id, Item item)
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
        public async Task<ActionResult> DeleteItem(string delete)
        {
            try
            {
                await Task.Run(() =>
                 {
                     string query = $"EXEC SP_DELETE @IDS= '{delete}'";
                     _context.Database.ExecuteSqlRaw(query);

                 });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        private bool ItemExists(int id)
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
