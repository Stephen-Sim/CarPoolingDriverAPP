using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarPoolingDriverAPP.ViewModels.Trip
{
    [QueryProperty(nameof(TripId), nameof(TripId))]
    public class SearchingTripPageViewModel : BindableObject
    {
        private string tripId;
        public string TripId
        {
            get { return tripId.ToString(); }
            set
            {
                tripId = Uri.UnescapeDataString(value ?? string.Empty);
                Map = new Map();
                this.LoadData();
            }
        }

        public TripService tripService { get; set; }

        private TripRequest trip;

        public TripRequest Trip
        {
            get { return trip; }
            set { trip = value; }
        }

        private List<RequestRequest> requestList;

        public List<RequestRequest> RequestList
        {
            get { return requestList; }
            set { requestList = value; OnPropertyChanged(); }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set { map = value; OnPropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        public SearchingTripPageViewModel()
        {
            Map = new Map();
            tripService = new TripService();
        }

        private Position FindMidPoint(double lat1, double lon1, double lat2, double lon2)
        {
            double dLon = (Math.PI / 180) * (lon2 - lon1);

            //convert to radians
            lat1 = (Math.PI / 180) * lat1;
            lat2 = (Math.PI / 180) * lat2;
            lon1 = (Math.PI / 180) * lon1;

            double Bx = Math.Cos(lat2) * Math.Cos(dLon);
            double By = Math.Cos(lat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);

            return new Position((180 / Math.PI) * lat3, (180 / Math.PI) * lon3);
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

        private async void LoadData()
        {
            var map = Map;
            Trip = await tripService.GetTrip(int.Parse(this.tripId));
            this.Title = $"{Trip.TripNumber} - {Trip.Status}";

            var StartPin = new Pin
            {
                Label = "My Start Location",
                Address = "",
                Type = PinType.Place,
                Position = new Position((double)Trip.FromLatitude, (double)Trip.FromLongitude)
            };

            var EndPin = new Pin
            {
                Label = "My Destination Location",
                Address = "",
                Type = PinType.Place,
                Position = new Position((double)Trip.ToLatitude, (double)Trip.ToLongitude)
            };

            var res = await tripService.GetTripsRequest(int.Parse(this.tripId));
            
            if (res != null)
            {
                RequestList = res;
            }

            if (RequestList != null)
            {
                foreach (var item in RequestList)
                {
                    var pin = new Pin
                    {
                        Label = item.PassengerName,
                        Type = PinType.Place,
                        Position = new Position((double)item.FromLatitude, (double)item.FromLongitude)
                    };

                    map.Pins.Add(pin);
                }
            }

            map.Pins.Add(StartPin);
            map.Pins.Add(EndPin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(FindMidPoint((double)Trip.FromLatitude, (double)Trip.FromLongitude, (double)Trip.ToLatitude, (double)Trip.ToLongitude), Distance.FromMiles(5)));

            Map = map;
        }

        public ICommand ConfirmTripButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    if (RequestList.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "There is no carpool request made!!", "Ok");
                        return;
                    }

                    bool isYes = await Application.Current.MainPage.DisplayAlert("Alert", "Are you sure to Confrim the Trip?", "Yes", "No");

                    if (isYes)
                    {
                        var confirmTrip = new ConfirmTripRequest();
                        confirmTrip.TripId = int.Parse(this.tripId);
                        confirmTrip.RequestsId = new List<int>();

                        foreach (var item in RequestList)
                        {
                            confirmTrip.RequestsId.Add((int)item.Id);
                        }

                        var res = await tripService.ConfirmTrip(confirmTrip);

                        if (res)
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "The Trip is Confirmed!!", "Ok");
                            await Shell.Current.Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "Operation Failed!!", "Ok");
                        }
                    }
                });
            }
        }
        
        public ICommand CancelTripButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    bool isYes = await Application.Current.MainPage.DisplayAlert("Alert", "Are you sure to Cancel the Trip?", "Yes", "No");

                    if (isYes)
                    {
                        var res = await tripService.CancelTrip(this.tripId);

                        if (res)
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "The Trip is Canceled!!", "Ok");
                            await Shell.Current.Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "Operation Failed!!", "Ok");
                        }
                    }
                });
            }
        }
    }
}
