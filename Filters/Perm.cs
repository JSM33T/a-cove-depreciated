using Almondcove.Interefaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace almondcove.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermAttribute : Attribute, IActionFilter
    {
        private readonly string[] _allowedRoles;

        public PermAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Executed after the action method
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Retrieve IPermissionService from the service provider
            IPermissionService permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>();

            if (permissionService == null)
            {
                throw new InvalidOperationException("IPermissionService not found in the service container.");
            }

            string currentRole = permissionService.CurrentRole(); // Consider refactoring to async

            bool isAuthorized = Array.Exists(_allowedRoles, role => role == currentRole);

            if (!isAuthorized)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            }
        }
    }
}
