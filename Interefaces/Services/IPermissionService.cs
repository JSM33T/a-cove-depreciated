using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almondcove.Interefaces.Services
{
    public interface IPermissionService
    {
        Task<bool> IsPageAllowed(int UserId);
        Task<bool> IsActionAllowed();
        public string CurrentRole();
    }
}
