using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleDto>> Register(RoleRequestModel model);
        Task<BaseResponse<RoleDto>> Get(string id);
        Task<BaseResponse<RoleDto>> GetUser(string RoleName);
        Task<BaseResponse<ICollection<RoleDto>>> GetAll();
    }
}
