using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.Common;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria;
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
    public class NotificationCriteriaController : ControllerBase
    {
        private readonly INotificationCriteriaService _notificationCriteriaService;

        public NotificationCriteriaController(INotificationCriteriaService notificationCriteriaService)
        {
            _notificationCriteriaService = notificationCriteriaService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<NotificationCriteriaResponse>>>> GetAllNotificationCriteriasAsync([FromQuery] NotificationCriteriaFilterAndPaginationRequest request)
        {
            var pagedNotificationCriterias = await _notificationCriteriaService.GetAllWithPaginationAsync<NotificationCriteriaResponse, NotificationCriteriaSpecification>(request);
            var response = new ApiResponse<PagedCollection<NotificationCriteriaResponse>>(pagedNotificationCriterias);
            return Ok(response);
        }

        // GET: api/NotificationCriterias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<NotificationCriteriaResponse>>> GetNotificationCriteria(int id)
        {
            var data = await _notificationCriteriaService.GetByIdProjectedAsync<NotificationCriteriaResponse>(id);
            var response = new ApiResponse<NotificationCriteriaResponse>(data);
            return Ok(response);
        }

        // PUT: api/NotificationCriterias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationCriteria(int id, NotificationCriteriaUpdateRequest request)
        {
            await _notificationCriteriaService.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/NotificationCriterias
        [HttpPost]
        public async Task<ActionResult<NotificationCriteriaResponse>> PostNotificationCriteria(NotificationCriteriaCreateRequest request)
        {
            var createdNotificationCriteria = await _notificationCriteriaService.CreateAsync<NotificationCriteriaCreateRequest, NotificationCriteriaResponse>(request);
            var response = new ApiResponse<NotificationCriteriaResponse>(createdNotificationCriteria);
            return CreatedAtAction("GetNotificationCriteria", new { id = createdNotificationCriteria.Id }, response);
        }

        // DELETE: api/NotificationCriterias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotificationCriteria(int id)
        {
            await _notificationCriteriaService.DeleteAsync(id);
            return NoContent();
        }

    }
}