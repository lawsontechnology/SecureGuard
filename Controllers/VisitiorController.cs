using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Visitor_Management_System.Core.Application;
using Visitor_Management_System.Core.Application.Authentication;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Enum;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : Controller
    {
        private readonly IVisitorService _visitorService;
        
        private readonly IJWTAuthenticationManager _authenticationManager;
        public VisitorController(IVisitorService visitorService,IJWTAuthenticationManager jWT)
        {
            _visitorService = visitorService;
            _authenticationManager = jWT;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var visitor = await _visitorService.Delete(id);
            if (visitor.Status == false)
            {
                return BadRequest(visitor);
            }
            return Ok(visitor);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var visitor = await _visitorService.GetAll();
            if (visitor == null)
            {
                return BadRequest(visitor);

            }
            return Ok(visitor.Data);
        }

        
        [HttpGet("Email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var visitor = await _visitorService.GetByEmail(email);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [HttpGet("HostEmail")]
        public async Task<IActionResult> GetByHostEmail([FromQuery] string hostEmail)
        {
            var visitor = await _visitorService.GetByHostEmail(hostEmail);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [HttpGet("AllApproved/Visit")]
        public async Task<IActionResult> GetVisitByHostEmail([FromQuery] string hostEmail)
        {
            var visitor = await _visitorService.GetVisitByHostEmail(hostEmail);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var visitor = await _visitorService.GetById(id);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [HttpGet("Visit/Id")]
        public async Task<IActionResult> GetVisit(string visitId)
        {
            var visit = await _visitorService.GetVisit(visitId);
            if (visit == null) return NotFound();
            return Ok(visit.Data);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(
                [FromForm][SwaggerParameter("Visitor request model")] VisitorRequestModel model,
               [FromForm][SwaggerParameter("Visit request model")] VisitRequestModel visitModel)
        {
            var visitor = await _visitorService.Register(model, visitModel);
            if (visitor.Status == false) return BadRequest(visitor);
            return Ok(visitor.Data);
        }

        /*[HttpGet("RedirectAfterAction")]
        public IActionResult RedirectAfterAction()
        {
            return Redirect("http://127.0.0.1:5505/index.html");
        }*/

        [HttpPost("Request/Approved")]
        public async Task<IActionResult> ApproveRequest([FromQuery] string token, [FromForm] string visitId)
        {

            
               var response =  await _visitorService.ApproveRequest(visitId, token);
            if(response.Status)
            {
                return Ok(response);
            }
            return BadRequest("Invalid token or already used");

        }

        [HttpPost("Request/Reject")]
        public async Task<IActionResult> RejectRequest([FromQuery] string token, [FromForm] string visitId)
        {

           var response = await _visitorService.DeniedRequest(visitId, token);
            if(response.Status)
            {
                return Ok(response);
            }
            return BadRequest("Invalid token or already used");

        }
    }

}
