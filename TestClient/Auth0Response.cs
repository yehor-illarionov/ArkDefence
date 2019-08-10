using RestSharp.Deserializers;

namespace TestClient
{
    partial class Program
    {
        public class Auth0Response
        {
            [DeserializeAs(Name = "access_token")]
            public string AccessToken { get; set; }
            [DeserializeAs(Name = "token_type")]
            public string TokenType { get; set; }
        }
    }
}
