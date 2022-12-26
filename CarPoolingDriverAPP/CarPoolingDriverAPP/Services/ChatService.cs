using CarPoolingDriverAPP.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CarPoolingDriverAPP.Services
{
    public class ChatService
    {
        private readonly HubConnection hubConnection;

        private string url = $"{APIConstant.APIURL}chat/";
        public HttpClient client { get; set; }
        public ChatService()
        {
            hubConnection = new HubConnectionBuilder().WithUrl($"{APIConstant.URL}chathub").Build();
            client = new HttpClient();
            var token = App.Current.Properties["token"] as string;
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("token", token);
        }
        public async Task<ObservableCollection<ChatMessage>> GetChatsByTripRequestId(string id)
        {
            try
            {
                string url = $"{this.url}GetChatsByTripRequestId?tripRequestId={id}";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ObservableCollection<ChatMessage>>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task Connect(int tripRequestId)
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("OnConnect", tripRequestId);
        }

        public async Task Disconnect(int tripRequestId)
        {
            await hubConnection.InvokeAsync("OnDisconnect", tripRequestId);
            await hubConnection.StopAsync();
        }

        public async Task SendMessage(int tripRequestId, string message)
        {
            await hubConnection.InvokeAsync("SendMessageToPassenger", tripRequestId, message);
        }

        public void ReceiveMessageFromPassenger(Action<int, string> GetMessageAndUser)
        {
            hubConnection.On("ReceiveMessageFromPassenger", GetMessageAndUser);
        }
    }
}
