using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepo _auditLog;
        public AuditLogService(IAuditLogRepo auditLog)
        {
            _auditLog = auditLog;
        }
        public async Task<BaseResponse<AuditLogDto>> Delete(string Id)
        {
            var Audit = await _auditLog.Get(Id);
            if(Audit == null)
            {
                return new BaseResponse<AuditLogDto>
                {
                    Message = "Log Not Found",
                    Status = false,
                };
            }
            Audit.IsDeleted = true;
            await _auditLog.SaveAsync();
            return new BaseResponse<AuditLogDto>
            {
                Message = "Log Is Successfully Deleted",
                Status = true,
            };
        }

        public async Task<BaseResponse<AuditLogDto>> Get(string UserId)
        {
            var Audit = await _auditLog.Get(x => x.UserId == UserId);
            if(Audit == null)
            {
                return new BaseResponse<AuditLogDto>
                {
                    Message = "Log Is Not Found",
                    Status = false,
                };
            }
            return new BaseResponse<AuditLogDto>
            {
                Status = true,
                Message = "Log Is Successfully Retrieved",
                Data = new AuditLogDto()
                {
                    Id = Audit.Id,
                  UserId = Audit.UserId,
                  Action = Audit.Action,
                  Timestamp = Audit.Timestamp,
                }
            };
        }

        public async Task<BaseResponse<ICollection<AuditLogDto>>> GetAll()
        {
            var auditLogs = await _auditLog.GetAll();
            var listOfAuditLogs = auditLogs.Select(audit => new AuditLogDto
            {
                Id = audit.Id,
                Action = audit.Action,
                Timestamp = audit.Timestamp,
                UserId = audit.UserId
            }).ToList();

            return new BaseResponse<ICollection<AuditLogDto>>
            {
                Status = true,
                Message = "Successful Retrieve All",
                Data = listOfAuditLogs,
            };
        }


        public async Task<BaseResponse<ICollection<AuditLogDto>>> GetAllDetails()
        {
            var audit = await _auditLog.GetAll();
            var listOfAuditLogs = audit.Select(auditLog => new AuditLogDto 
            {
                Id = auditLog.Id,
                UserId = auditLog.UserId,
                 Timestamp = auditLog.Timestamp,
                Action = auditLog.Action,
            }).ToList();

            return new BaseResponse<ICollection<AuditLogDto>>
            {
                Status = true,
                Message = "Successful Retrieve All",
                Data = listOfAuditLogs,
            };
        }

        




    }
}
