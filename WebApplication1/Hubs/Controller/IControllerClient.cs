using System.Collections.Generic;
using System.Threading.Tasks;


namespace WebApplication1.Hubs
{
    public interface IControllerClient
    {
        #region huinya
        Task GetFingerTimeout();
        Task GetUsers(List<string> users);
        #endregion

        Task SetFingerTimeout(SetFingerTimeoutReq req);
        Task GetFingerTimeoutCurrent(GetFingerTimeoutReq req);
        Task AddFinger(AddFingerReq req);
        Task SendConfig(SendConfigReq req);
        Task GetConfig(GetConfigReq req);
        Task ReceiveConfig(GetConfigRes res);
        Task DeleteAllFingerprints(DeleteAllFingerprintsReq req);
        Task DeleteFingerById(DeleteFingerByIdReq req);
        Task AddFingerByBle(AddFingerByBleReq req);
    }
}
