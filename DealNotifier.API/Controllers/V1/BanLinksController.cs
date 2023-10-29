using Catalog.Application.Interfaces.Services;
using Catalog.Application.Specification;
using Catalog.Application.ViewModels.V1.BanLink;
using Catalog.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BanLinksController : ControllerBase
    {
        private readonly IBanLinkService _banLinkService;

        public BanLinksController(IBanLinkService banLinkService)
        {
            _banLinkService = banLinkService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<BanLinkResponse>>>> GetAllBanLinksAsync([FromQuery] BanLinkPaginationRequest request)
        {
            var pagedBanLinks = await _banLinkService.GetAllWithPaginationAsync<BanLinkResponse, BanLinkSpecification>(request);
            var response = new ApiResponse<PagedCollection<BanLinkResponse>>(pagedBanLinks);
            return Ok(response);
        }

        // GET: api/BanLinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BanLinkResponse>>> GetBanLink(int id)
        {
            var data = await _banLinkService.GetByIdProjectedAsync<BanLinkResponse>(id);
            var response = new ApiResponse<BanLinkResponse>(data);
            return Ok(response);
        }

        // PUT: api/BanLinks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanLink(int id, BanLinkUpdateRequest request)
        {
            await _banLinkService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/BanLinks
        [HttpPost]
        public async Task<ActionResult<BanLinkResponse>> PostBanLink(BanLinkCreateRequest request)
        {
            var createdBanLink = await _banLinkService.CreateAsync<BanLinkCreateRequest, BanLinkResponse>(request);
            var response = new ApiResponse<BanLinkResponse>(createdBanLink);
            return CreatedAtAction("GetBanLink", new { id = createdBanLink.Id }, response);
        }

        // DELETE: api/BanLinks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBanLink(int id)
        {
            await _banLinkService.DeleteAsync(id);
            return NoContent();
        }

    }
}