using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessConnectManagement
{
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}