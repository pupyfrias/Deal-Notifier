using ApiGateway.Attributes;
using ApiGateway.Interfaces;
using ApiGateway.Wrappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserProto;
namespace ApiGateway.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("{id}/roles")]
        [RequireCreatePermission]
        public async Task<ActionResult> AddRolesAsync(Guid id, [FromBody] List<string> roles)
        {
            await _userService.AddRolesAsync(id, roles);
            return NoContent();
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<UserCreateResponse>> CreateAsync([FromBody] UserCreateRequest request)
        {
            return await _userService.CreateAsync(request);
        }

        [HttpDelete("{id}")]
        [RequireDeletePermission]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        
        [HttpGet]
        [RequireReadPermission]
        public async Task<ActionResult<IEnumerable<UserList>>> GetAllUsers([FromQuery] QueryParametersRequest queryParameters)
        {
            var pagedUsersResult = await _userService.GetAllAsync(queryParameters);
            return Ok(pagedUsersResult);
        }

        [HttpGet("{id}")]
        [RequireReadPermission]
        public async Task<ActionResult<UserDetailsResponse>> GetAsync(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("{id}/roles")]
        [RequireReadPermission]
        public async Task<ActionResult<IList<string>>> GetRolesAsync(Guid id)
        {
            var roles = await _userService.GetRolesAsync(id);
            return Ok(roles);
        }

        [HttpDelete("{id}/roles")]
        [RequireDeletePermission]
        public async Task<ActionResult> RemoveRolesAsync(Guid id, [FromBody] List<string> roles)
        {
            await _userService.RemoveRolesAsync(id, roles);
            return NoContent();
        }

        [HttpPut("{id}")]
        [RequireUpdatePermission]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UserUpdateRequest request)
        {
            await _userService.UpdateAsync(id, request);
            return NoContent();
        }

    }
}

