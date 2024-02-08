using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Controllers
{
    /*[Authorize(Roles = "Admin")]*/
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm] RoleRequestModel request)
        {
            var isSuccessful = await _roleService.Register(request);
            if (!isSuccessful.Status)
            {
                return BadRequest(isSuccessful.Message);
            }
            return Ok(isSuccessful);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            if (roles == null)
            {
                return BadRequest(roles);
            }
            return Ok(roles.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            var role = await _roleService.Get(id);
            if (role == null)
            {
                return BadRequest(role.Message);
            }
            return Ok(role.Data);
        }

        [HttpGet("Get/Names")]
        public async Task<IActionResult> GetRoles([FromForm] string roleName)
        {
            var role = await _roleService.GetUser(roleName);
            if (role == null)
            {
                return BadRequest(role.Message);
            }
            return Ok(role);
        }
    }

}
