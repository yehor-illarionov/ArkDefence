namespace WebApplication1.Hubs
{
    #region client requests

    public class AddFingerReq
    {
        public int Uid { get; set; }
        public int Privilage { get; set; }
        public string Port { get; set; }
    }

    #endregion
}
