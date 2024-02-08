using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IExcelService
    {
        Task<BaseResponse<IEnumerable<UserDto>>> ImportAndSave(IFormFile file, string roleId);
    }
}
