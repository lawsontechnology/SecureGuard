using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>> Login(LoginRequestModel model);
        Task<BaseResponse<UserDto>> RegisterAdmin(AdminProfileRequestModel model);
        /*Task<BaseResponse<ICollection<UserDto>>> SaveUsersToDatabase(List<UserDto> users, List<ProfileDto> profiles);*/
        Task<BaseResponse<UserDto>> Update(string id, UpdateProfileRequestModel model);
        Task<BaseResponse<UserDto>> GetById(string id);
        Task<BaseResponse<UserDto>> Get(string email);
        Task<BaseResponse<ICollection<UserDto>>> GetAllSecurity();
        Task<BaseResponse<ICollection<UserDto>>> GetAllHost();
        Task<BaseResponse<ICollection<UserDto>>> GetAll();
        Task<BaseResponse<UserDto>> UserRegister(RegisterRequestModel model);
        Task<BaseResponse<UserDto>> Delete(string Id);
    }
}
