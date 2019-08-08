using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArkDefence.AspNetCore.Host.Data;
using ArkDefence.AspNetCore.Host.Models;
using ArkDefence.AspNetCore.Host.Models.Events;
using Coravel.Events.Interfaces;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ArkDefence.AspNetCore.Host.Hubs
{
    public static class UserHandler
    {
        public static List<string> ConnectedIds = new List<string>();
    }

    [Authorize]
    public class ControllerHub : Hub<IControllerClient>
    {
        //private IDispatcher coravel_dispathcer;
        private readonly IQueue _queue;
        private readonly ApplicationDbContext _dbcontext;

        public ControllerHub(IQueue queue, ApplicationDbContext dbcontext)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            //dbcontext.EnsureAutoHistory();
        }

        public async Task SetFingerTimeoutUser(string user, int timeout)
        {
            _queue.QueueBroadcast<MessageReceived>(new MessageReceived (Context.UserIdentifier, "SetFingerTimeoutUser", $"user:{user}, timeout:{timeout}"));
            await Clients.Client(user).SetFingerTimeout(timeout);
        }
        public async Task SetFingerTimeout(int timeout)
        {
            _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "SetFingerTimeout", $"timeout:{timeout}"));
            await Clients.All.SetFingerTimeout(timeout);
        }

        public async Task GetFingerTimeout(string userid)
        {
            _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetFingerTimeout", $"userid:{userid}"));
            await Clients.User(userid).GetFingerTimeout();
        }

        public async Task GetUsers()
        {
            _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetUsers", ""));
            var temp = _dbcontext.App_MessageHistory.Last();
            temp.Method = temp.Method + ": auto hsitory test";
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
            await Clients.Caller.GetUsers(UserHandler.ConnectedIds);
        }

        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.UserIdentifier);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public interface IControllerClient
    {
        Task SetFingerTimeout(int timeout);
        Task GetFingerTimeout();
        Task GetUsers(List<string> users);
    }
}
