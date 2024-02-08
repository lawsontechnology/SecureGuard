using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _role;
        public RoleService(IRoleRepo role)
        {
            _role = role;
        }
        public async Task<BaseResponse<RoleDto>> Get(string id)
        {
            var role = await _role.Get(id);
            if (role == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Not Found",
                    Status = false,
                };
            }
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Retrieved",
                Status = true,
                Data = new RoleDto(role.Id)
                { 
                    Name = role.Name,
                    Description = role.Description,
                    DateCreated = role.DateCreated,
                },
            };
        }

        public async Task<BaseResponse<ICollection<RoleDto>>> GetAll()
        {
            List<RoleDto> listOfRole = new List<RoleDto>();
            var rol = await _role.GetAll();
            foreach (var roles in rol)
            {
                var role = new RoleDto(roles.Id)
                {

                    Name = roles.Name,
                    Description = roles.Description,
                    DateCreated = roles.DateCreated,
                };
                listOfRole.Add(role);
            }

            return new BaseResponse<ICollection<RoleDto>>
            {
                Status = true,
                Message = "List of Role",
                Data = listOfRole,
            };
        }

        public async Task<BaseResponse<RoleDto>> GetUser(string RoleName)
        {
            var role = await _role.Get(x => x.Name == RoleName && x.IsDeleted == false);
            if (role == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Not Found",
                    Status = false,
                };
            }
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Retrieved",
                Status = true,
                Data = new RoleDto(role.Id)
                {
                    Name = role.Name,
                    Description = role.Description,
                    DateCreated = role.DateCreated,
                },
            };
        }

        public async Task<BaseResponse<RoleDto>> Register(RoleRequestModel model)
        {
            var role = await _role.Get(x => x.Name == model.Name && x.IsDeleted == false);
            if (role != null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Already Exist",
                    Status = false
                };
            }
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
            };

            var isSuccessful = await _role.CreateAsync(newRole);
            if (isSuccessful == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Unable to Add Role",
                    Status = false
                };
            }
            await _role.SaveAsync();
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Created",
                Status = true,
                Data = new RoleDto(newRole.Id)
                {
                    /*Id = newRole.Id,*/
                    Name = newRole.Name,
                    Description = newRole.Description,
                    DateCreated = role.DateCreated,

                }
            };
        }
    }
}
