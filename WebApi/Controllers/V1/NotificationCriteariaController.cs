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
        private readonly INotificationCriteriaServiceAsync _notificationCriteriaServiceAsync;

        public NotificationCriteriaController(INotificationCriteriaServiceAsync notificationCriteriaServiceAsync)
        {
            _notificationCriteriaServiceAsync = notificationCriteriaServiceAsync;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedCollection<NotificationCriteriaResponse>>>> GetAllNotificationCriteriasAsync([FromQuery] NotificationCriteriaFilterAndPaginationRequest request)
        {
            var pagedNotificationCriterias = await _notificationCriteriaServiceAsync.GetAllWithPaginationAsync<NotificationCriteriaResponse, NotificationCriteriaSpecification>(request);
            var response = new ApiResponse<PagedCollection<NotificationCriteriaResponse>>(pagedNotificationCriterias);
            return Ok(response);
        }

        // GET: api/NotificationCriterias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<NotificationCriteriaResponse>>> GetNotificationCriteria(int id)
        {
            var data = await _notificationCriteriaServiceAsync.GetByIdProjectedAsync<NotificationCriteriaResponse>(id);
            var response = new ApiResponse<NotificationCriteriaResponse>(data);
            return Ok(response);
        }

        // PUT: api/NotificationCriterias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationCriteria(int id, NotificationCriteriaUpdateRequest request)
        {
            await _notificationCriteriaServiceAsync.UpdateAsync(id, request);
            return NoContent();
        }

        // POST: api/NotificationCriterias
        [HttpPost]
        public async Task<ActionResult<NotificationCriteriaResponse>> PostNotificationCriteria(NotificationCriteriaCreateRequest request)
        {
            var createdNotificationCriteria = await _notificationCriteriaServiceAsync.CreateAsync<NotificationCriteriaCreateRequest, NotificationCriteriaResponse>(request);
            var response = new ApiResponse<NotificationCriteriaResponse>(createdNotificationCriteria);
            return CreatedAtAction("GetNotificationCriteria", new { id = createdNotificationCriteria.Id }, response);
        }

        // DELETE: api/NotificationCriterias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotificationCriteria(int id)
        {
            await _notificationCriteriaServiceAsync.DeleteAsync(id);
            return NoContent();
        }

    }
}