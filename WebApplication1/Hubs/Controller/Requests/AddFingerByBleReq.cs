namespace WebApplication1.Hubs
{
    #region client requests

    public class AddFingerByBleReq
    {
        public string UserId { get; set; }
        public string Ble { get; set; }
        public int Id { get; set; }
        public int Privilage { get; set; }
        public string Port { get; set; }
    }

    #endregion
}
