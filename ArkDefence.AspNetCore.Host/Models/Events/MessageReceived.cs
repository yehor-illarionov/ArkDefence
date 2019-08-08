using Coravel.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Models.Events
{
    public class MessageReceived:IEvent
    {
        public string UserIdentifier { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }

        public MessageReceived(string user, string method, string data)
        {
            UserIdentifier = user;
            Method = method;
            Data = data;
        }
    }
}
