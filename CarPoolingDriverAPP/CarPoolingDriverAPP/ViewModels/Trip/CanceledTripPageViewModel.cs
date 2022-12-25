using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using CarPoolingDriverAPP.Views.Home;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.StyleSheets;

namespace CarPoolingDriverAPP.ViewModels.Trip
{
    [QueryProperty(nameof(TripId), nameof(TripId))]
    public class CanceledTripPageViewModel : BindableObject
    {

        public CanceledTripPageViewModel()
        {
            Map = new Map();
            tripService = new TripService();
        }

        public TripService tripService { get; set; }

        private TripRequest trip;

        public TripRequest Trip
        {
            get { return trip; }
            set { trip = value; }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set { map = value; OnPropertyChanged(); }
        }

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

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
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

            map.Pins.Add(StartPin);
            map.Pins.Add(EndPin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(FindMidPoint((double)Trip.FromLatitude, (double)Trip.FromLongitude, (double)Trip.ToLatitude, (double)Trip.ToLongitude), Distance.FromMiles(5)));

            Map = map;
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
        public ICommand PlaceTheRequestButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    var content = JsonConvert.SerializeObject(this.Trip);
                    await Shell.Current.GoToAsync($"Home/{nameof(ConfirmTripPage)}?StringRequest={content}");
                });
            }
        }
    }
}
