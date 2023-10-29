using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.BanKeyword;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> PutBanKeyword(int id, BanKeywordUpdateRequest request)
        {
            await _banKeywordService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/BanKeywords
        [HttpPost]
        public async Task<ActionResult<BanKeywordResponse>> PostBanKeyword(BanKeywordCreateRequest request)
        {
            var createdBanKeyword = await _banKeywordService.CreateAsync<BanKeywordCreateRequest, BanKeywordResponse>(request);
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
