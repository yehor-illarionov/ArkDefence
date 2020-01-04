namespace WebApplication1.Hubs
{
    #region client requests
    public class SetFingerTimeoutReq
    {
        public int Timeout { get; set; }
        public string Port { get; set; }
    }

    #endregion
}
