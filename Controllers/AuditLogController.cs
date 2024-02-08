using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLog)
        {
            _auditLogService = auditLog;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] string id)
        {
            var log = await _auditLogService.Get(id);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
        }
 
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var log = await _auditLogService.GetAll();
            if (log == null)
            {
                return BadRequest(log);
            }
            return Ok(log.Data);
        }


        [HttpGet("GetAll/Details")]
        public async Task<IActionResult> GetAllDetailsAsync()
        {
            var log = await _auditLogService.GetAllDetails();
            if (log == null)
            {
                return BadRequest(log);
            }
            return Ok(log.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var log = await _auditLogService.Delete(id);
            if (!log.Status)
            {
                return BadRequest(log);
            }
            return NoContent(); 
        }



    }

}
