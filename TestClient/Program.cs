using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using RestSharp.Deserializers;

namespace TestClient
{
    class Program
    {
        static HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:5001/hubs/controllerhub", options=> 
               {
                   options.AccessTokenProvider = async()=> await GetAccessToken();
                  // options.
               })
               .WithAutomaticReconnect()
               .Build();

        static SecurityToken validatedToken=null;
        static string token = null;
        const string Domain = "https://dev-3ru57p69.eu.auth0.com/";
        const string Audience = "https://localhost:5001";

        static IConfigurationManager<OpenIdConnectConfiguration> configurationManager = 
            new ConfigurationManager<OpenIdConnectConfiguration>($"{Domain}.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
        static OpenIdConnectConfiguration openIdConfig = configurationManager.GetConfigurationAsync(CancellationToken.None).Result;

        static TokenValidationParameters validationParameters =
            new TokenValidationParameters
            {
                ValidIssuer = Domain,
                ValidAudiences = new[] { Audience },
                IssuerSigningKeys = openIdConfig.SigningKeys,
                ValidateLifetime = true
            };


        static Users clients = new Users() { List = new List<string>() };
        static async Task Main(string[] args)
        {

            clients.PropertyChanged += UsersChanged;

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.Reconnecting += error =>
            {
                Debug.Assert(connection.State == HubConnectionState.Reconnecting);

                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.
                Console.WriteLine("Reconnecting");

                return Task.CompletedTask;
            };

            connection.Reconnected += connectionId =>
            {
                Debug.Assert(connection.State == HubConnectionState.Connected);

                // Notify users the connection was reestablished.
                // Start dequeuing messages queued while reconnecting if any.
                Console.WriteLine("Reconnected");

                return Task.CompletedTask;
            };

            connection.On<int>("SetFingerTimeout", timeout =>
            {
                Console.WriteLine($"Set timeout from server:{timeout}");
            });


            connection.On<List<string>>("GetUsers", users =>
            {
                clients.List = users;
                foreach (var user in users)
                {
                    Console.WriteLine($"User:{user}");
                }
            });

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            await ConnectWithRetryAsync(connection, cancelTokenSource.Token);
            await connection.InvokeAsync("GetUsers");
            await connection.InvokeAsync("SetFingerTimeout", 3000);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
            cancelTokenSource.Cancel();
        }

        static async void UsersChanged(object sender, PropertyChangedEventArgs args)
        {
            Console.WriteLine("Users changed");
            foreach (var user in clients.List)
            {
                Console.WriteLine($"user: {user}");
                await connection.InvokeAsync("SetFingerTimeoutUser", user, 5000);
                //await connection.InvokeAsync("SetFingerTimeout", 3000);
                Console.WriteLine("Invoked");
            }
        }

        public static async Task<string> GetAccessToken()
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            Console.WriteLine("Access_Token");
            if (validatedToken != null & token!=null)
            {
                //JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                Console.WriteLine($"static {validatedToken.ToString()}");
                var user = handler.ValidateToken(token, validationParameters, out validatedToken);
              //  Console.WriteLine($"Token is validated. User Id {user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
                return token;
            }
            else
            {
                var client = new RestClient("https://dev-3ru57p69.eu.auth0.com/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                var apiId = HttpUtility.UrlEncode("https://localhost:5001");
                var secret = "0-eKlEelGGShg5pK8Gdgvb8lYVfR6UCjpzhxD1U41D11fNLBQmt78bP6ESyFFS4d";
                var clientid = "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI";
                request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientid}&client_secret={secret}&audience={apiId}", ParameterType.RequestBody);
                // request.AddParameter("application/json", "{\"client_id\":\"DYaPShg0nOEptG3AIeDgNBCudk7w3LhI\",\"client_secret\":\"0-eKlEelGGShg5pK8Gdgvb8lYVfR6UCjpzhxD1U41D11fNLBQmt78bP6ESyFFS4d\",\"audience\":\"https://dev-3ru57p69.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
                var response = client.Execute<Auth0Response>(request);
                Console.WriteLine($"{response.Data.AccessToken}");
                var user = handler.ValidateToken(response.Data.AccessToken, validationParameters, out validatedToken);
                token = response.Data.AccessToken;
                return response.Data.AccessToken;
            }
        }

        public class Auth0Response
        {
            [DeserializeAs(Name = "access_token")]
            public string AccessToken { get; set; }
            [DeserializeAs(Name = "token_type")]
            public string TokenType { get; set; }
        }

        public static async Task<bool> ConnectWithRetryAsync(HubConnection connection, CancellationToken token)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    await connection.StartAsync(token);
                    Debug.Assert(connection.State == HubConnectionState.Connected);
                    Console.WriteLine("Connected");
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(connection.State == HubConnectionState.Disconnected);
                    Console.WriteLine("Trying to connect");
                    await Task.Delay(5000);
                }
            }
        }
    }

    class Users : INotifyPropertyChanged
    {
        private List<string> list;
        public List<string> List
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                OnPropertyChanged("List");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
