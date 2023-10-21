using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BanKeywordsController : ControllerBase
    {
        private readonly IBanKeywordService _banKeywordService;

        public BanKeywordsController(IBanKeywordService banKeywordService)
        {
            _banKeywordService = banKeywordService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<BanKeywordResponse>>>> GetAllBanKeywordsAsync([FromQuery] BanKeywordFilterAndPaginationRequest request)
        {
            var pagedBanKeywords = await _banKeywordService.GetAllWithPaginationAsync<BanKeywordResponse, BanKeywordSpecification>(request);
            var response = new ApiResponse<PagedCollection<BanKeywordResponse>>(pagedBanKeywords);
            return Ok(response);
        }

        // GET: api/BanKeywords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BanKeywordResponse>>> GetBanKeyword(int id)
        {
            var data = await _banKeywordService.GetByIdProjectedAsync<BanKeywordResponse>(id);
            var response = new ApiResponse<BanKeywordResponse>(data);
            return Ok(response);
        }

        // PUT: api/BanKeywords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanKeyword(int id, BanKeywordUpdateRequest banKeyword)
        {
            await _banKeywordService.UpdateAsync(id, banKeyword);
            return NoContent();
        }

        // POST: api/BanKeywords
        [HttpPost]
        public async Task<ActionResult<BanKeywordResponse>> PostBanKeyword(BanKeywordCreateRequest banKeyword)
        {
            var createdBanKeyword = await _banKeywordService.CreateAsync<BanKeywordCreateRequest, BanKeywordResponse>(banKeyword);
            var response = new ApiResponse<BanKeywordResponse>(createdBanKeyword);
            return CreatedAtAction("GetBanKeyword", new { id = createdBanKeyword.Id }, response);
        }

        // DELETE: api/BanKeywords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBanKeyword(int id)
        {
            await _banKeywordService.DeleteAsync(id);
            return NoContent();
        }
    }
}