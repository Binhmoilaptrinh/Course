using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CustomAuthorizationService : ICustomAuthorizationService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public CustomAuthorizationService(IUserRoleRepository userRoleRepository, IUserPermissionRepository userPermissionRepository, IPermissionRepository permissionRepository, IUserRepository userRepository, IRolePermissionRepository rolePermissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userPermissionRepository = userPermissionRepository;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task AssignRoleToUser(int userId, int roleId)
        {
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            await _userRoleRepository.AddAsync(userRole);
        }

        public async Task AssignPermissionToUser(int userId, int permissionId)
        {
            var userPermission = new UserPermission { UserId = userId, PermissionId = permissionId };
            await _userPermissionRepository.AddAsync(userPermission);
        }

        public async Task<bool> CheckUserPermission(int userId, string permissionName)
        {
            var permissions = await _permissionRepository.GetAllAsync();
            var permission = permissions.FirstOrDefault(p => p.PermissionName == permissionName);
            if (permission == null) return false;

            var userPermissions = await _userPermissionRepository.GetAllAsync();
            return userPermissions.Any(up => up.UserId == userId && up.PermissionId == permission.PermissionId);
        }

        public async Task<bool> AuthorizeAsync(int userId, string permissionName)
        {
            var userRoles = await _userRepository.GetUserRolesAsync(userId);

            foreach (var userRole in userRoles) // Duyệt qua từng UserRole
            {
                if (await _rolePermissionRepository.CheckPermissionAsync(userRole.RoleId, permissionName)) // Lấy RoleId từ UserRole
                {
                    return true;
                }
            }

            return false;
        }
    }

}
