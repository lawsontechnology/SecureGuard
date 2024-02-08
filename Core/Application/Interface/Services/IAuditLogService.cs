
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IAuditLogService
    {
        Task<BaseResponse<AuditLogDto>> Get(string UserId);
        Task<BaseResponse<ICollection<AuditLogDto>>> GetAllDetails();
        Task<BaseResponse<ICollection<AuditLogDto>>> GetAll();
        Task<BaseResponse<AuditLogDto>> Delete(string Id);
        
    }
}
