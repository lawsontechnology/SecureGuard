using Visitor_Management_System.Core.Domain.Enum;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IVisitorService
    {
        Task<BaseResponse<VisitorDto>> Register(VisitorRequestModel model, VisitRequestModel visitModel);
        Task<BaseResponse<VisitorDto>> GetById(string id);
        Task<BaseResponse<VisitorDto>> GetByEmail(string email);
        Task<BaseResponse<VisitorDto>> Delete(string Id);
        Task<BaseResponse<ICollection<VisitDto>>> GetVisitByHostEmail(string hostEmail);
        Task<BaseResponse<ICollection<VisitorDto>>> GetByHostEmail(string hostEmail);
        Task<BaseResponse<ICollection<VisitorDto>>> GetAll();
        Task<BaseResponse<VisitorDto>> GetVisit(string visitId);
        Task<BaseResponse<VisitDto>> ApproveRequest(string visitId, string token);
        Task<BaseResponse<VisitDto>> DeniedRequest(string visitId, string token);
        
    }
}
