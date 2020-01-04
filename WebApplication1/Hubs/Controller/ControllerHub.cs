using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;


namespace WebApplication1.Hubs
{

    [Authorize]
    public class ControllerHub : Hub<IControllerClient>
    {
        //private IDispatcher coravel_dispathcer;
        //private readonly IMultiTenantStore _dbcontext;

        public ControllerHub()
        {
            //_dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            //dbcontext.EnsureAutoHistory();
        }

        public async Task GetTimeoutCurrent(string address, string port)
        {
            // _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetTimeoutCurrent", $"user:{address}, port:{port}"));
            await Clients.User(address).GetFingerTimeoutCurrent(new GetFingerTimeoutReq { Port = port });
        }

        public async Task AddFingerTo(string address, int uid, int privilage, string port)
        {
            await Clients.User(address).AddFinger(new AddFingerReq { Port = port, Privilage = privilage, Uid = uid });
        }

        public async Task SendConfigTo(string address, string json, string port)
        {
            await Clients.User(address).SendConfig(new SendConfigReq { JsonString = json, Port = port });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">autho0 client id</param>
        /// <param name="timeout">0&lt;x&lt;255</param>
        /// <param name="port">linux system port e.g. /dev/ttyS0</param>
        /// <returns></returns>
        public async Task SetFingerTimeoutTo(string address, int timeout, string port)
        {
            await Clients.User(address).SetFingerTimeout(new SetFingerTimeoutReq { Timeout = timeout, Port = port });
        }

        public async Task DeleteFingerByIdTo(string address, int id, string port)
        {
            await Clients.User(address).DeleteFingerById(new DeleteFingerByIdReq { Id = id, Port = port });
        }

        public async Task DeleteAllFingerprintsTo(string address, string port)
        {
            await Clients.User(address).DeleteAllFingerprints(new DeleteAllFingerprintsReq { Port = port });
        }

        public async Task AddFingerByBleTo(string address, string userid, string ble, int id, int privilage, string port)
        {
            await Clients.User(address).AddFingerByBle(new AddFingerByBleReq { UserId = userid, Ble = ble, Id = id, Port = port, Privilage = privilage });
        }
        public async Task GetFingerTimeout(string address)
        {
            //  _queue.QueueBroadcast<MessageReceived>(new MessageReceived(Context.UserIdentifier, "GetFingerTimeout", $"userid:{userid}"));
            await Clients.User(address).GetFingerTimeout();
        }

        public async Task GetConfigFrom(string address, string port)
        {
            await Clients.User(address).GetConfig(new GetConfigReq { Port = port, Address=address });
        }

        public async Task SendConfigResTo(string address, string json, string port)
        {
            await Clients.User(address).ReceiveConfig(new GetConfigRes { JsonString = json, Port=port });
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
}
