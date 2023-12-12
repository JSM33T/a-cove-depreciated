using almondcove.Enums;
using System.Security;

namespace almondcove.Modules
{
    public static class PermissionHelper
    {
        public static bool HasPermission(string userRole, Perm requiredPermission)
        {
            return userRole switch
            {
                "guest" => requiredPermission == Perm.Guest,
                "user" => requiredPermission == Perm.User,
                "editor" => requiredPermission == Perm.User || requiredPermission == Perm.Edit,
                "admin" => true,
                _ => false,
            };
        }
    }
}
