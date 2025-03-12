namespace WebAPI.Services.Interfaces
{
    public interface ICustomAuthorizationService
    {
        //Task AssignRoleToUser(int userId, int roleId);
        //Task AssignPermissionToUser(int userId, int permissionId);
        //Task<bool> CheckUserPermission(int userId, string permissionName);

        Task<bool> AuthorizeAsync(int userId, string permissionName);
    }
}
