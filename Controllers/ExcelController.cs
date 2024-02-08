using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
using Visitor_Management_System.Core.Application.Implementation.Services;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExcelService _excelService;

        public ExcelController(IUserService userService, IExcelService excelService)
        {
            _userService = userService;
            _excelService = excelService;
        }

        [HttpPost("Export/ExcelTemplate")]
        public  IActionResult ExportExcelTemplate()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var wb = new XLWorkbook())
                    {
                        var empSheet = wb.AddWorksheet("User Records");

                        //  headers 
                        var headers = new[] { "S/N", "Email", "FirstName", "LastName", "PhoneNumber" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            empSheet.Cell(1, i + 1).Value = headers[i];
                        }

                        wb.SaveAs(ms);
                    }

                    HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "SecureGuard_Template.xlsx"
                    };
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error exporting Excel template: {ex.Message}");
                return new BadRequestResult();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ImportAndSave")]
         public async Task<IActionResult> ImportAndSave(IFormFile file, [FromForm] string roleId)
         {
            var result = await _excelService.ImportAndSave(file, roleId);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
    }




}
