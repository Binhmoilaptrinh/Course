using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Services.Interfaces;

namespace WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permission;

        public AuthorizePermissionAttribute(string permission)
        {
            _permission = permission;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetService<ICustomAuthorizationService>();
            var user = context.HttpContext.User;

            if (authorizationService == null || user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            bool hasPermission = await authorizationService.AuthorizeAsync(userId, _permission);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }


    }
}
