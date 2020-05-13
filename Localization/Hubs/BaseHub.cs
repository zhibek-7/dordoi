using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Localization.Hubs
{
    public abstract class BaseHub : Hub
    {

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

    }
}
