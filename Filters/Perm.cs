using Almondcove.Interefaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace almondcove.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermAttribute(params string[] allowedRoles) : Attribute, IActionFilter
    {
        private readonly string[] _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IPermissionService _permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>() ?? throw new InvalidOperationException("IPermissionService not found in the service container.");
            string currentRole = _permissionService.CurrentRole();

            bool isAuthorized = Array.Exists(_allowedRoles, role => role == currentRole);

            if (!isAuthorized) context.Result = new ObjectResult("Unauthorized Access")
            {
                StatusCode = 401
            };
        }
    }
}
