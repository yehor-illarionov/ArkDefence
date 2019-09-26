using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;


namespace WebApplication1.Hubs
{
    public static class UserHandler
    {
        public static List<string> ConnectedIds = new List<string>();
    }

    [Authorize]
    public class ControllerHub : Hub<IControllerClient>
    {
        //private IDispatcher coravel_dispathcer;
        private readonly ApplicationDbContext _dbcontext;

        public ControllerHub(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            //dbcontext.EnsureAutoHistory();
        }

        public async Task GetTimeoutCurrent(string address, string port)
        {
           // _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetTimeoutCurrent", $"user:{address}, port:{port}"));
            await Clients.User(address).GetFingerTimeoutCurrent(port);
        }

        public async Task AddFingerTo(string address, int uid, int privilage)
        {
            await Clients.User(address).AddFinger(uid, privilage);
        }

        #region huinya
        public async Task SetFingerTimeoutUser(string user, int timeout)
        {
          //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "SetFingerTimeoutUser", $"user:{user}, timeout:{timeout}"));
            await Clients.Client(user).SetFingerTimeout(timeout);
        }
        public async Task SetFingerTimeout(int timeout)
        {
          //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "SetFingerTimeout", $"timeout:{timeout}"));
            await Clients.All.SetFingerTimeout(timeout);
        }

        public async Task GetFingerTimeout(string userid)
        {
          //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetFingerTimeout", $"userid:{userid}"));
            await Clients.User(userid).GetFingerTimeout();
        }

        public async Task GetUsers()
        {
          //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetUsers", ""));
            //  ..var temp = _dbcontext.App_MessageHistory.Last();
            //temp.Method = temp.Method + ": auto hsitory test";
            //  _dbcontext.Update(temp);
            //  _dbcontext.EnsureAutoHistory();
            //  await _dbcontext.SaveChangesAsync();
            await Clients.Caller.GetUsers(UserHandler.ConnectedIds);
        }
        #endregion
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
        #region huinya
        Task SetFingerTimeout(int timeout);
        Task GetFingerTimeout();
        Task GetUsers(List<string> users);
        #endregion
        Task GetFingerTimeoutCurrent(string port);
        Task AddFinger(int uid, int privilage);
    }
}
