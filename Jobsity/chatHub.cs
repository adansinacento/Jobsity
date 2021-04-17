using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobsity
{
    public class chatHub : Hub
    {
        public void SendToClients(string elem)
        {
            //elem its actually the element to append
            Clients.All.message(elem);
        }
    }
}