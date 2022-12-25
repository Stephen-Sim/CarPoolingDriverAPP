using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using CarPoolingDriverAPP.Views.Home;
using CarPoolingDriverAPP.Views.Trip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CarPoolingDriverAPP.ViewModels.Trip
{
    public class TripPageViewModel : BindableObject
    {
        private List<TripRequest> requests;

        public List<TripRequest> Requests
        {
            get { return requests; }
            set { requests = value; OnPropertyChanged(); }
        }

        public TripService requestService { get; set; }

        public TripPageViewModel()
        {
            Requests = new List<TripRequest>();
            requestService = new TripService();
            this.LoadData();
        }

        private async void LoadData()
        {
            var res = await requestService.GetTrips();
            if (res != null)
            {
                Requests = res;
            }
        }

        private bool isRefreshing = false;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public ICommand ListViewRereshed
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;
                    this.LoadData();
                    IsRefreshing = false;
                });
            }
        }

        public Command ListViewItemTapped
        {
            get
            {
                return new Command(async (sender) => {
                    var trip = (TripRequest) sender;

                    if (trip.Status == "Searching")
                    {
                        await Shell.Current.GoToAsync($"Trip/{nameof(SearchingTripPage)}?TripId={trip.Id}");
                    }
                    else if (trip.Status == "Pending")
                    {
                        await Shell.Current.GoToAsync($"Trip/{nameof(PendingTripPage)}?TripId={trip.Id}");
                    }
                    else if (trip.Status == "Canceled")
                    {
                        await Shell.Current.GoToAsync($"Trip/{nameof(CanceledTripPage)}?TripId={trip.Id}");
                    }
                    else if (trip.Status == "Completed")
                    {
                        await Shell.Current.GoToAsync($"Trip/{nameof(CompletedTripPage)}?TripId={trip.Id}");
                    }
                });
            }
        }
    }
}
