using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Almondcove.Interefaces.Services;

namespace Almondcove.Services
{
    public class PermissionService : IPermissionService
    {
        public Task<string> CurrentRole(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsActionAllowed()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPageAllowed(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}