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

        public async Task SendConfigTo(string address, string json)
        {
            await Clients.User(address).SendConfig(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">autho0 client id</param>
        /// <param name="timeout">0<x<255</param>
        /// <param name="port">linux system port e.g. /dev/ttyS0</param>
        /// <returns></returns>
        public async Task SetFingerTimeoutTo(string address, int timeout, string port)
        {
            await Clients.User(address).SetFingerTimeout(timeout, port);
        }

        public async Task DeleteFingerByIdTo(string address, int id, string port)
        {
            await Clients.User(address).DeleteFingerById(id, port);
        }

        public async Task DeleteAllFingerprintsTo(string address, string port)
        {
            await Clients.User(address).DeleteAllFingerprints(port);
        }

        public async Task AddFingerByBleTo(string address, string userid, string ble, int id, int privilage, string port)
        {
            await Clients.User(address).AddFingerByBle(userid, ble, id, privilage, port);
        }
        public async Task GetFingerTimeout(string userid)
        {
            //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetFingerTimeout", $"userid:{userid}"));
            await Clients.User(userid).GetFingerTimeout();
        }

        #region huinya

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
        Task GetFingerTimeout();
        Task GetUsers(List<string> users);
        #endregion
        Task SetFingerTimeout(int timeout, string port);
        Task GetFingerTimeoutCurrent(string port);
        Task AddFinger(int uid, int privilage);
        Task SendConfig(string json);
        Task DeleteAllFingerprints(string port);
        Task DeleteFingerById(int id, string port);
        Task AddFingerByBle(string userid, string ble, int id, int privilage, string port);
    }
}
