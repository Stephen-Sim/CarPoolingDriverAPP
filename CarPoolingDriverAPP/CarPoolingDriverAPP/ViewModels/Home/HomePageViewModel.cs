using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Views.Home;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarPoolingDriverAPP.ViewModels.Home
{
    [QueryProperty(nameof(StringRequest), nameof(StringRequest))]
    public class HomePageViewModel : BindableObject
    {

        private Map map;
        public Map Map
        {
            get { return map; }
            set { map = value; OnPropertyChanged(); }
        }

        private string stringRequest = string.Empty;
        public string StringRequest
        {
            get { return stringRequest; }
            set
            {
                if (value == null)
                {
                    Map = new Map() { IsShowingUser = true };
                    Map.Pins.Clear();
                    stringRequest = string.Empty;
                    ButtonTextColor = "Gray";
                    ButtonText = "Place your Request";
                    ToButtonText = "To: Destination Location";
                    FromButtonText = "From: Current Location";
                    return;
                }

                stringRequest = Uri.UnescapeDataString(value ?? string.Empty);
                Request = JsonConvert.DeserializeObject<TripRequest>(stringRequest);

                var StartPin = new Pin
                {
                    Label = "Start Location",
                    Address = "",
                    Type = PinType.Place,
                    Position = new Position((double)Request.FromLatitude, (double)Request.FromLongitude)
                };

                var EndPin = new Pin
                {
                    Label = "Destination Location",
                    Address = "",
                    Type = PinType.Place,
                    Position = new Position((double)Request.ToLatitude, (double)Request.ToLongitude)
                };

                Map.Pins.Add(StartPin);
                Map.Pins.Add(EndPin);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(FindMidPoint((double)Request.FromLatitude, (double)Request.FromLongitude, (double)Request.ToLatitude, (double)Request.ToLongitude), Distance.FromMiles(5)));

                ButtonTextColor = "Black";

                FromButtonText = Request.FromAddress.Length > 33 ? Request.FromAddress.Substring(0, 28) + "...." : Request.FromAddress;
                ToButtonText = Request.ToAddress.Length > 33 ? Request.ToAddress.Substring(0, 28) + "...." : Request.ToAddress;
            }
        }

        public TripRequest Request { get; set; }

        private string buttonText = "Place your Request";
        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; OnPropertyChanged(); }
        }

        private string buttonTextColor = "Gray";
        public string ButtonTextColor
        {
            get { return buttonTextColor; }
            set { buttonTextColor = value; OnPropertyChanged(); }
        }

        private string fromButtonText = "From: Current Location";

        public string FromButtonText
        {
            get { return fromButtonText; }
            set { fromButtonText = value; OnPropertyChanged(); }
        }

        private string toButtonText = "To: Destination Location";

        public string ToButtonText
        {
            get { return toButtonText; }
            set { toButtonText = value; OnPropertyChanged(); }
        }
        public HomePageViewModel()
        {
            Map = new Map { IsShowingUser= true };
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

        public ICommand PinLacationStackLayoutTapped
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"Home/{nameof(PinStartLocationPage)}");
                });
            }
        }

        public ICommand PlaceRequestButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    if (string.IsNullOrEmpty(stringRequest))
                    {
                        await Shell.Current.GoToAsync($"Home/{nameof(PinStartLocationPage)}");
                    }
                    else
                    {
                        var content = JsonConvert.SerializeObject(this.Request);
                        await Shell.Current.GoToAsync($"Home/{nameof(ConfirmTripPage)}?StringRequest={content}");
                    }
                });
            }
        }
    }
}
