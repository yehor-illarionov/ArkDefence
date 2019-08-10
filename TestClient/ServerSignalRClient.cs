using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static TestClient.Program;

namespace TestClient
{
    public class ServerSignalRClient
    {
        private const string Domain = "https://dev-3ru57p69.eu.auth0.com/";
        private const string Audience = "https://localhost:5001";
        private const string Secret = "0-eKlEelGGShg5pK8Gdgvb8lYVfR6UCjpzhxD1U41D11fNLBQmt78bP6ESyFFS4d";
        private const string ClientId = "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI";
        private const string ConnectionUri = "https://localhost:5001/hubs/controllerhub";

        private string token=null;
        private SecurityToken validatedToken=null;

        private readonly IConfigurationManager<OpenIdConnectConfiguration> configurationManager;
        private readonly OpenIdConnectConfiguration openIdConfig;
        private readonly TokenValidationParameters validationParameters;
        private readonly HubConnection connection;
        public HubConnection Connection { get => connection; private set{ return; } }

        public ServerSignalRClient()
        {
            configurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>
                    ($"{Domain}.well-known/openid-configuration", 
                    new OpenIdConnectConfigurationRetriever());
            openIdConfig= configurationManager.GetConfigurationAsync(CancellationToken.None).Result;
            validationParameters =
                new TokenValidationParameters
                {
                    ValidIssuer = Domain,
                    ValidAudiences = new[] { Audience },
                    IssuerSigningKeys = openIdConfig.SigningKeys,
                    ValidateLifetime = true
                };
            connection= new HubConnectionBuilder()
               .WithUrl(ConnectionUri, options =>
               {
                   options.AccessTokenProvider = async () => await GetAccessToken();
                   // options.
               })
               .WithAutomaticReconnect()
               .Build();
            connection.Closed += async(error) =>await ConnectionClosed(error);
            connection.Reconnected += connectionId => ConnectionReconnected(connectionId);
            connection.Reconnecting += error => ConnectionReconnecting(error);
        }

        public async Task StartAsync(CancellationToken token)
        {
            await ConnectWithRetryAsync(connection, token);
        }

        private async Task ConnectionClosed(Exception error)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        }

        private Task ConnectionReconnected(string connectionId)
        {
            Debug.Assert(connection.State == HubConnectionState.Connected);
            // Notify users the connection was reestablished.
            // Start dequeuing messages queued while reconnecting if any.
            Console.WriteLine("SignalR Reconnected");
            return Task.CompletedTask;
        }

        private Task ConnectionReconnecting(Exception error)
        {
            Debug.Assert(connection.State == HubConnectionState.Reconnecting);
            // Notify users the connection was lost and the client is reconnecting.
            // Start queuing or dropping messages.
            Console.WriteLine("Reconnecting");
            return Task.CompletedTask;
        }

        public async Task<string> GetAccessToken()
        {
            Console.WriteLine($"Token");
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            if (validatedToken != null & token != null)
            {
                Console.WriteLine($"static {validatedToken.ToString()}");
                var user = handler.ValidateToken(token, validationParameters, out validatedToken);
                return token;
            }
            else
            {
               // Console.WriteLine($"2");
                var client = new RestClient($"{Domain}oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                var apiId = HttpUtility.UrlEncode(Audience);
                request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={ClientId}&client_secret={Secret}&audience={apiId}", ParameterType.RequestBody);
                var response = client.Execute<Auth0Response>(request);
                Console.WriteLine($"{response.Data.AccessToken}");
                var user = handler.ValidateToken(response.Data.AccessToken, validationParameters, out validatedToken);
                token = response.Data.AccessToken;
                return response.Data.AccessToken;
            }
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
}
