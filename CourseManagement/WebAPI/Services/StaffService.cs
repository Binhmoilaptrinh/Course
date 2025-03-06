using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebAPI.DTOS.reponse;
using WebAPI.DTOS.request;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class StaffService : IStaffService
    {
        public readonly IUserRepository _repo;
        public readonly IUserRoleRepository _repoUR;
        public readonly IMapper _mapper;
        public readonly ISendEmail _emailSender;
        public readonly IAuthenticateService _authenticateService;

        public StaffService(IUserRepository repo, IUserRoleRepository repoUR, IMapper mapper, ISendEmail emailSender, IAuthenticateService authenticateService)
        {
            _repo = repo;
            _repoUR = repoUR;
            _mapper = mapper;
            _emailSender = emailSender;
            _authenticateService = authenticateService;
        }

        public async Task<List<StaffReponseDto>> GetStaffReponses()
        {
            var listStaff = await _repo.GetAllAsync();

            var staffResponses = listStaff.Select(user => new StaffReponseDto
            {
                UserId = user.UserId,
                UserName = user.User.Username,
                Email = user.User.Email,
                SelectedRole = user.UserId, // Lấy RoleId hoặc mặc định là 0
                RoleName = user.Role.RoleName // Lấy tên Role nếu có
            }).ToList();

            return staffResponses;
        }


        public async Task<StaffReponseDto> AddStaff(StaffRequestDto staff)
        {

            var newUser = _mapper.Map<User>(staff);
            var checkU = await _repo.CheckAsyncForUserName(staff.UserName);
            if (checkU)
            {
                throw new Exception($"User with {staff.UserName} is existed");
            }
            var checkE = await _repo.CheckAsyncForEmail(staff.Email);
            if (checkE)
            {
                throw new Exception($"User with {staff.Email} is existed");
            }
            var password = GenerateRandomPassword();
            newUser.Password = await _authenticateService.HashPasswordAsync(password);
            var createdUser = await _repo.CreateAsync(newUser);
            if (createdUser != null)
            {
                await _emailSender.SendEmailAsync(createdUser.Email, "Account is created", $"Here is your new password {password}");

                // Kiểm tra role hợp lệ
                var roleExists = await _repoUR.CheckUserRoleAsync(staff.SelectedRole);
                if (!roleExists)
                {
                    throw new Exception($" {staff.SelectedRole} is invalid");
                }

                var userRole = await _repoUR.CreateAsync(new UserRole { UserId = createdUser.UserId, RoleId = staff.SelectedRole });
                if (userRole == null)
                {
                    throw new Exception($" {staff.SelectedRole} is invalid");
                }
            }
            var staffReponse = _mapper.Map<StaffReponseDto>(createdUser);
            staffReponse.SelectedRole = staff.SelectedRole;
            return staffReponse;

        }

        public async Task<StaffReponseDto> UpdateStaff(StaffReponseDto staff)
        {
            var existingUser = await _repo.GetAsync(staff.UserId); // Lấy thông tin user hiện tại
            if (existingUser == null)
            {
                throw new Exception($"User with ID {staff.UserId} not found");
            }

            var checkU = await _repo.GetUserAsyncForUserName(staff.UserName);
            if (checkU != null && checkU.UserId != staff.UserId)  // Kiểm tra username có bị trùng với user khác không
            {
                throw new Exception($"User with {staff.UserName} is existed");
            }

            var checkE = await _repo.GetUserAsyncForEmail(staff.Email);
            if (checkE != null && checkE.UserId != staff.UserId)  // Kiểm tra email có bị trùng với user khác không
            {
                throw new Exception($"User with {staff.Email} is existed");
            }

            // Cập nhật thông tin user
            existingUser.Username = staff.UserName;
            existingUser.Email = staff.Email;

            var updatedUser = await _repo.UpdateAsync(existingUser);
            //if (updatedUser != null)
            //{
            //    // Kiểm tra role hợp lệ
            //    var roleExists = await _repoUR.CheckUserRoleAsync(staff.SelectedRole);
            //    if (!roleExists)
            //    {
            //        throw new Exception($"{staff.SelectedRole} is invalid");
            //    }

            //    var userRole = await _repoUR.UpdateAsync(new UserRole { UserId = updatedUser.UserId, RoleId = staff.SelectedRole });
            //    if (userRole == null)
            //    {
            //        throw new Exception($"{staff.SelectedRole} is invalid");
            //    }
            //}

            var staffResponse = _mapper.Map<StaffReponseDto>(updatedUser);
            staffResponse.SelectedRole = staff.SelectedRole;
            return staffResponse;
        }


        public static string GenerateRandomPassword(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

       
    }
}
