using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarPoolingDriverAPP.ViewModels.Trip.Chat
{
    [QueryProperty(nameof(Content), nameof(Content))]

    public class ChatPassengerPageViewModel : BindableObject
    {
        public ChatPassengerPageViewModel()
        {
            Chats = new ObservableCollection<ChatMessage>();
            tripService = new TripService();
            chatService = new ChatService();
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand 
        { 
            get
            {
                return new Command(async () => {
                    if (!string.IsNullOrEmpty(Message))
                    {
                        await chatService.SendMessage(int.Parse(TripRequestId), Message);
                        var tempList = Chats;
                        tempList.Add(new ChatMessage { Message = message, IsYourMessage = true, IsNotYourMessage = false });
                        Chats = new ObservableCollection<ChatMessage>(tempList);
                        Message = string.Empty;
                    }
                });
            }
        }

        public ChatService chatService { get; set; }

        private ObservableCollection<ChatMessage> chat;
        public ObservableCollection<ChatMessage> Chats
        {
            get { return chat; }
            set { chat = value; OnPropertyChanged(); }
        }

        public TripService tripService { get; set; }

        private TripRequest trip;
        public TripRequest Trip
        {
            get { return trip; }
            set { trip = value; }
        }

        private string content;
        public string Content
        {
            get { return content.ToString(); }
            set
            {
                content = Uri.UnescapeDataString(value ?? string.Empty);
                var res = content.Split(',');
                TripId = res[0];
                TripRequestId = res[1];
                this.LoadData();
            }
        }

        private string tripId;
        public string TripId
        {
            get { return tripId; }
            set { tripId = value; OnPropertyChanged(); }
        }

        private string tripRequestId;
        public string TripRequestId
        {
            get { return tripRequestId; }
            set { tripRequestId = value; OnPropertyChanged(); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        private async void LoadData()
        {
            Trip = await tripService.GetTrip(int.Parse(this.tripId));
            this.Title = $"{Trip.TripNumber} - {Trip.Status}";

            try
            {
                var res = await chatService.GetChatsByTripRequestId(this.TripRequestId);
                if (res != null)
                {
                    Chats = res;
                }

                await chatService.Connect(int.Parse(this.TripRequestId));
                chatService.ReceiveMessageFromPassenger((tripRequestId, message) =>
                {
                    if (tripRequestId == int.Parse(this.tripRequestId))
                    {
                        var tempList = Chats;
                        tempList.Add(new ChatMessage { Message = message, IsYourMessage = false, IsNotYourMessage = true });
                        Chats = new ObservableCollection<ChatMessage>(tempList);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
