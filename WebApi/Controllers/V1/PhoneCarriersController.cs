using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    //[Authorize(Roles = $"{Role.SuperAdmin}")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PhoneCarriersController : ControllerBase
    {
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;

        public PhoneCarriersController(IPhoneCarrierService phoneCarrierService, IUnlockabledPhonePhoneCarrierService unlockabledPhonePhoneCarrierService)
        {
            _phoneCarrierService = phoneCarrierService;
            _unlockabledPhonePhoneCarrierService = unlockabledPhonePhoneCarrierService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<PhoneCarrierResponse>>>> GetAllPhoneCarriersAsync([FromQuery] PhoneCarrierFilterAndPaginationRequest request)
        {
            var pagedPhoneCarriers = await _phoneCarrierService.GetAllWithPaginationAsync<PhoneCarrierResponse, PhoneCarrierSpecification>(request);
            var response = new ApiResponse<PagedCollection<PhoneCarrierResponse>>(pagedPhoneCarriers);
            return Ok(response);
        }

        // GET: api/PhoneCarriers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PhoneCarrierResponse>>> GetPhoneCarrier(int id)
        {
            var data = await _phoneCarrierService.GetByIdProjectedAsync<PhoneCarrierResponse>(id);
            var response = new ApiResponse<PhoneCarrierResponse>(data);
            return Ok(response);
        }

        // PUT: api/PhoneCarriers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneCarrier(int id, PhoneCarrierUpdateRequest request)
        {
            await _phoneCarrierService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/PhoneCarriers
        [HttpPost]
        public async Task<ActionResult<PhoneCarrierResponse>> PostPhoneCarrier(PhoneCarrierCreateRequest request)
        {
            var createdPhoneCarrier = await _phoneCarrierService.CreateAsync<PhoneCarrierCreateRequest, PhoneCarrierResponse>(request);
            var response = new ApiResponse<PhoneCarrierResponse>(createdPhoneCarrier);
            return CreatedAtAction("GetPhoneCarrier", new { id = createdPhoneCarrier.Id }, response);
        }

        // DELETE: api/PhoneCarriers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhoneCarrier(int id)
        {
            await _phoneCarrierService.DeleteAsync(id);
            return NoContent();
        }

    }
}