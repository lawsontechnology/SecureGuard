using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Visitor_Management_System.Core.Application.Authentication;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;

        public UserController(IUserService userService, IJWTAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("Register/Admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] AdminProfileRequestModel request)
        {
            var user = await _userService.RegisterAdmin(request);
            return user.Status ? Ok(user) : NotFound();
        }

        /*[Authorize(Roles = "Admin")]*/
        [HttpPost("Register/User")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterRequestModel request)
        {
            var user = await _userService.UserRegister(request);
            return user.Status ? Ok(user) : NotFound();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateProfileRequestModel request)
        {
            var user = await _userService.Update(id, request);
            return user.Status ? Ok(user) : NotFound();
        }

        [HttpGet("Id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var user = await _userService.GetById(id);
            return user.Status ? Ok(user) : BadRequest(user);
        }

        [Authorize (Roles ="Admin") ]
        [HttpGet("GetAll/Securities")]
        public async Task<IActionResult> GetAllSecurities()
        {
            var user = await _userService.GetAllSecurity();
            return user.Status ? Ok(user.Data) : BadRequest(user);
        }
         
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/Hosts")]
        public async Task<IActionResult> GetAllHost()
        {
            var user = await _userService.GetAllHost();
            return user.Status ? Ok(user.Data) : BadRequest(user.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll/Users")]
        public async Task<IActionResult> GetAllAsync()
        {
            var user = await _userService.GetAll();
            return user.Status ? Ok(user.Data) : BadRequest(user.Message);
        }

        [HttpGet("Get/UserEmail")]
        public async Task<IActionResult> GetUserEmail([FromQuery] string email)
        {
            var user = await _userService.Get(email);
            return user.Status ? Ok(user) : BadRequest(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            var user = await _userService.Delete(id);
            return user.Status ? Ok(user) : BadRequest(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn( LoginRequestModel model)
        {
            var user = await _userService.Login(model);
            if (user != null)
            {
                var token = _jwtAuthenticationManager.GenerateToken(user.Data);
                var response = new LoginResponseModel(user.Data.Id, token, user.Data.Email,user.Data.FirstName,user.Data.LastName, user.Data.RoleName, user.Data.PhoneNumber,user.Data.UserRoles);
               
                return Ok(response);
            }

            return BadRequest(user);
        }  


    }
}
