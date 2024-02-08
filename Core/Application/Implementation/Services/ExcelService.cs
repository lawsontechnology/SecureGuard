using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Visitor_Management_System.Core.Application.Email;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IUserRepo _user;
        private readonly IRoleRepo _role;
        private readonly IProfileRepo _profile;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailServices _email;
        
        public ExcelService(IUserRepo user, IProfileRepo profile, IRoleRepo role, IMailServices email) 
        {
            _user = user;
            _profile = profile;
            _role = role;
            _email = email;
            
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> ImportAndSave(IFormFile file, string RoleId)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheets.First();
                    var totalRows = worksheet.RowCount();

                    var listOfUserEntities = new List<User>();

                    for (var rowNum = 2; rowNum <= totalRows; rowNum++)
                    {
                        
                        var userEntity = new User
                        {
                            
                            Email = worksheet.Cell(rowNum, 2).Value.ToString(),
                            PassWord = BCrypt.Net.BCrypt.HashPassword("12345"),
                            DateCreated = DateTime.Now,
                        };

                        if (_user.Check(u => u.Email == userEntity.Email))
                        {
                            /*continue;*/
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Message = $"{userEntity.Email} Already Exist !!",
                                Status = false,
                                
                            };
                        }

                        if (string.IsNullOrEmpty(userEntity.Email))
                        {
                            
                            break;
                        }


                        var roleId = RoleId;
                        var role = await _role.Get(x => x.Id == roleId && x.IsDeleted == false);
                        if (role == null)
                        {
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Status = false,
                                Message = $"Error occurred while saving the data. Role '{roleId}' does not exist."
                            };
                        }

                        if (!IsValidEmail(userEntity.Email))
                        {
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Status = false,
                                Message = $"Invalid email format: {userEntity.Email}.",
                            };
                        }
                        var userRole = new UserRole
                        {
                            Role = role,
                            RoleId = role.Id,
                            DateCreated = DateTime.Now,
                        };

                        userEntity.UserRoles = new List<UserRole> { userRole };

                        var profileEntity = new Profile
                        {
                            
                            FirstName = worksheet.Cell(rowNum, 3).Value.ToString(),
                            LastName = worksheet.Cell(rowNum, 4).Value.ToString(),
                            PhoneNumber = worksheet.Cell(rowNum, 5).Value.ToString(),
                            UserId = userEntity.Id,

                            DateCreated = DateTime.Now,
                        };

                        userEntity.Profile = profileEntity;
                        
                        await _user.CreateAsync(userEntity);
                        await _profile.CreateAsync(profileEntity);

                        SendRegisterRequestEmail(userEntity, profileEntity);

                    }

                    
                    await _user.SaveAsync();
                    await _profile.SaveAsync();

                    var listOfUserDto = listOfUserEntities.Select(MapToUserDto);

                    return new BaseResponse<IEnumerable<UserDto>>
                    {
                        Status = true,
                        Message = "Data imported and saved successfully.",
                        Data = listOfUserDto
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserDto>>
                {
                    Status = false,
                    Message = $"Error importing and saving data: {ex.Message}",
                    Data = null
                };
            }
        }

        private bool IsValidEmail(string email)
        {
            const string emailRegex = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        // Email 
        private void SendRegisterRequestEmail(User user, Profile profile)
        {
            string receiverEmail = user.Email;
            string receiverName = $"{profile.FirstName} {profile.LastName}";
            string subject = "SecureGuard Notification";
            string message = $"<html><body><h2>Pls Kindly Register Using This Link</h2></body></html>\n" +
                             $"<html><body><h3>Update Your Details After Login Is Successful</h3></body></html> :\n" +
                             $"<html><body><h5>FirstName: {profile.FirstName}</h5></body></html>\n" +
                             $"<html><body><h5>LastName: {profile.LastName}</h5></body></html>\n" +
                             $"<html><body><h5>Email: {user.Email}</h5></body></html>\n" +
                             $"<html><body><h5>PhoneNumber: {profile.PhoneNumber}</h5></body></html>\n" +
                             $"<html><body><h5>Password:12345</h5></body></html>\n" +
                             $"<html><body><h4>Registration Link: http://127.0.0.1:5505/login.html</h4></body></html>";

            
            var mailRequest = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,
                
            };

            _email.SendEMail(mailRequest);
        }


        private UserDto MapToUserDto(User userEntity)
        {
            return new UserDto(userEntity.Id)
            {
                Email = userEntity.Email,
                PassWord = userEntity.PassWord,
                FirstName = userEntity.Profile.FirstName,
                LastName = userEntity.Profile.LastName,
                PhoneNumber = userEntity.Profile.PhoneNumber,
                RoleName = userEntity.UserRoles?.FirstOrDefault()?.Role?.Name,
                Profile = userEntity.Profile,
                 
            };
        }




    }
}
