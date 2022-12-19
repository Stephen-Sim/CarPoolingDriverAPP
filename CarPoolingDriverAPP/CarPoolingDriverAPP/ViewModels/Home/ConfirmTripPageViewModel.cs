using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Views.Home;
using CarPoolingDriverAPP.Services;
using System.Threading.Tasks;

namespace CarPoolingDriverAPP.ViewModels.Home
{
    [QueryProperty(nameof(StringRequest), nameof(StringRequest))]
    public class ConfirmTripPageViewModel : BindableObject
    {
        private TripRequest request;
        public TripRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        private string stringRequest = string.Empty;
        public string StringRequest
        {
            get { return stringRequest; }
            set
            {
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
            }
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

        public Map Map { get; set; }

        private List<Vehicle> vehichlesList;

        public List<Vehicle> VehichlesList
        {
            get { return vehichlesList; }
            set { vehichlesList = value; OnPropertyChanged(); }
        }

        public Vehicle SelectedVehichle { get; set; }

        public ConfirmTripPageViewModel()
        {
            Map = new Map();
            requestService = new TripService();
            vehicleService = new VehicleService();
            VehichlesList = new List<Vehicle>();
            SelectedVehichle = new Vehicle();

            this.LoadVehiclesAsync();
        }

        private async void LoadVehiclesAsync()
        {
            var res = await vehicleService.GetVehicles();

            if (res.Count != 0)
            {
                VehichlesList = new List<Vehicle>(res);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "You have not uploaded any vehicle info yet!", "Ok");
            }
        }

        public ICommand CancelButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"///{nameof(HomePage)}");
                });
            }
        }

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public TimeSpan SelectedTime { get; set; }

        public VehicleService vehicleService { get; set; }

        public TripService requestService { get; set; }

        public ICommand ConfirmButtonClicked
        {
            get
            {
                return new Command(async () =>
                {

                    if (SelectedDate <= DateTime.Today)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "You can't Select Passed Date or Today", "Ok");
                        return;
                    }

                    if (SelectedVehichle == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "You have to select a vehicle!!", "Ok");
                        return;
                    }

                    Request.VehicleId = SelectedVehichle.Id;
                    Request.Date = SelectedDate;
                    Request.Time = SelectedTime;

                    var res = await requestService.CreateTrip(this.Request);

                    if (res)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "The Request is Successfully Created!!", "Ok");
                        await Shell.Current.GoToAsync($"///{nameof(HomePage)}");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Operation Failed!!", "Ok");
                    }
                });
            }
        }
    }
}
