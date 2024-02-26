using Asp.Versioning;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1.Brand;
using DealNotifier.Core.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DealNotifier.API.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<BrandResponse>>>> GetAllBrandsAsync([FromQuery] BrandFilterAndPaginationRequest request)
        {
            var pagedBrands = await _brandService.GetAllWithPaginationAsync<BrandResponse, BrandSpecification>(request);
            var response = new ApiResponse<PagedCollection<BrandResponse>>(pagedBrands);
            return Ok(response);
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BrandResponse>>> GetBrand(int id)
        {
            var data = await _brandService.GetByIdProjectedAsync<BrandResponse>(id);
            var response = new ApiResponse<BrandResponse>(data);
            return Ok(response);
        }

        // PUT: api/Brands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, BrandUpdateRequest request)
        {
            await _brandService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/Brands
        [HttpPost]
        public async Task<ActionResult<BrandResponse>> PostBrand(BrandCreateRequest request)
        {
            var createdBrand = await _brandService.CreateAsync<BrandCreateRequest, BrandResponse>(request);
            var response = new ApiResponse<BrandResponse>(createdBrand);
            return CreatedAtAction("GetBrand", new { id = createdBrand.Id }, response);
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteAsync(id);
            return NoContent();
        }

    }
}