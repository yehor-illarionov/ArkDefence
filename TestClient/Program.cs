using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    partial class Program
    {
        private static ServerSignalRClient client = new ServerSignalRClient();
        private static Users clients = new Users() { List = new List<string>() };

        private static async Task Main(string[] args)
        {
            clients.PropertyChanged += UsersChanged;

            client.Connection.On<dynamic>("SetFingerTimeout", req =>
            {
                Console.WriteLine($"Set timeout from server:{req.Timeout}");
            });

            client.Connection.On<List<string>>("GetUsers", users =>
            {
                clients.List = users;
                foreach (var user in users)
                {
                    Console.WriteLine($"User:{user}");
                }
            });

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            await client.StartAsync(cancelTokenSource.Token);
           // await client.Connection.InvokeAsync("GetUsers");
            await client.Connection.InvokeAsync("SetFingerTimeoutTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", 5, "/dev/ttyS1");
          //  await client.Connection.InvokeAsync("SetFingerTimeoutTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", 5, "/dev/ttyS2");
            await client.Connection.InvokeAsync("GetTimeoutCurrent", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", "/dev/ttyS1");
           // await client.Connection.InvokeAsync("GetTimeoutCurrent", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", "/dev/ttyS2");
             await client.Connection.InvokeAsync("AddFingerTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", 1, 1, "/dev/ttyS1");
               await client.Connection.InvokeAsync("SendConfigTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", "{\"Name\":\"Test\",\"mode\":1,\"delay\":5,\"BLE_State\":1}", "/dev/ttyS1");
               await client.Connection.InvokeAsync("DeleteFingerByIdTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", 1, "/dev/ttyS1");
             await client.Connection.InvokeAsync("AddFingerByBleTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients",
                  "some_user", "1BFF1801BEAC2F234454CF6D4A0FADF2F4911BA9FFA600010002BF00", 1, 1, "/dev/ttyS1");
               await client.Connection.InvokeAsync("DeleteAllFingerprintsTo", "DYaPShg0nOEptG3AIeDgNBCudk7w3LhI@clients", "/dev/ttyS1");
            Console.ReadLine();
            cancelTokenSource.Cancel();
        }

        private static async void UsersChanged(object sender, PropertyChangedEventArgs args)
        {
            Console.WriteLine("Users changed");
            foreach (var user in clients.List)
            {
                Console.WriteLine($"user: {user}");
                // await client.Connection.InvokeAsync("SetFingerTimeoutUser", user, 5000);
                //await connection.InvokeAsync("SetFingerTimeout", 3000);
                Console.WriteLine("Invoked");
            }
        }
    }
}

internal class Users : INotifyPropertyChanged
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