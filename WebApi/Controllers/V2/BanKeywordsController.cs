using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BanKeywordsController : ControllerBase
    {
        private readonly IBanKeywordServiceAsync _banKeywordServiceAsync;

        public BanKeywordsController(IBanKeywordServiceAsync banKeywordServiceAsync)
        {
            _banKeywordServiceAsync = banKeywordServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<BanKeywordResponse>>>> GetAllBanKeywordsAsync([FromQuery] BanKeywordFilterAndPaginationRequest request)
        {
            var pagedBanKeywords = await _banKeywordServiceAsync.GetAllWithPaginationAsync<BanKeywordResponse, BanKeywordSpecification>(request);
            var response = new ApiResponse<PagedCollection<BanKeywordResponse>>(pagedBanKeywords);
            return Ok(response);
        }

        // GET: api/BanKeywords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BanKeywordResponse>>> GetBanKeyword(int id)
        {
            var data = await _banKeywordServiceAsync.GetByIdProjectedAsync<BanKeywordResponse>(id);
            var response = new ApiResponse<BanKeywordResponse>(data);
            return Ok(response);
        }

        // PUT: api/BanKeywords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanKeyword(int id, BanKeywordUpdateRequest banKeyword)
        {
            await _banKeywordServiceAsync.UpdateAsync(id, banKeyword);
            return NoContent();
        }

        // POST: api/BanKeywords
        [HttpPost]
        public async Task<ActionResult<BanKeyword>> PostBanKeyword(BanKeywordCreateRequest banKeyword)
        {
            var createdBanKeyword = await _banKeywordServiceAsync.CreateAsync(banKeyword);
            var response = new ApiResponse<BanKeyword>(createdBanKeyword);
            return CreatedAtAction("GetBanKeyword", new { id = createdBanKeyword.Id }, response);
        }

        // DELETE: api/BanKeywords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBanKeyword(int id)
        {
            await _banKeywordServiceAsync.DeleteAsync(id);
            return NoContent();
        }
    }
}