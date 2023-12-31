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
            IPermissionService permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>() ?? throw new InvalidOperationException("IPermissionService not found in the service container.");
            string currentRole = permissionService.CurrentRole();

            bool isAuthorized = Array.Exists(_allowedRoles, role => role == currentRole);

            //if (!isAuthorized) context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            if (!isAuthorized)
            {
                // Assuming "AccessDenied" is the name of your view
                ViewResult viewResult = new()
                {
                    ViewName = "~/Views/Home/AccessDenied.cshtml"
                };

                context.Result = viewResult;
            }
        }
    }
}
