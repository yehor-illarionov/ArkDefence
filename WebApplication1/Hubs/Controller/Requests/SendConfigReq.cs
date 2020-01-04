namespace WebApplication1.Hubs
{
    #region client requests

    public  class SendConfigReq
    {
        public string JsonString { get; set; }
        public string Port { get; set; }
    }

    #endregion
}
