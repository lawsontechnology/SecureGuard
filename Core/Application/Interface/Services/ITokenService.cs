using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface ITokenService
    {
        Task<Token> GetById(string id);
         Task<string> GenerateApproveToken();
        Task<BaseResponse<TokenDto>> ValidateAndRemoveToken(string token);

    }
}
