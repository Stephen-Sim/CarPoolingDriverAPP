using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Newtonsoft.Json;
using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Views.Home;
using System.Linq;

namespace CarPoolingDriverAPP.ViewModels.Home
{
    public class PinStartLocationPageViewModel : BindableObject
    {
        private readonly Geocoder geocoder = new Geocoder();
        public Map Map { get; set; }
        public Position Position { get; set; } = new Position();

        public ICommand PinLocationButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    var address = (await geocoder.GetAddressesForPositionAsync(this.Position)).FirstOrDefault()?.ToString();
                    address = address.Replace('&', ',');
                    bool result = await Application.Current.MainPage.DisplayAlert("Confirm the Location", $"{(address.Length > 100 ? address.Substring(0, 97) + "..." : address)}", "YES", "NO");

                    if (result)
                    {
                        var StartLocation = new TripRequest
                        {
                            FromLatitude = (decimal)this.Position.Latitude,
                            FromLongitude = (decimal)this.Position.Longitude,
                            FromAddress = address
                        };

                        var content = JsonConvert.SerializeObject(StartLocation);

                        await Shell.Current.GoToAsync($"Home/{nameof(PinEndLocationPage)}?StartLocation={content}");
                    }
                });
            }
        }

        public string AddressInserted { get; set; } = "";

        public ICommand SearchButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    var location = (await Xamarin.Essentials.Geocoding.GetLocationsAsync($"{AddressInserted}")).FirstOrDefault();

                    if (location != null)
                    {
                        Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", $"Location Not Found!!", "Ok");
                    }
                });
            }
        }

        public PinStartLocationPageViewModel()
        {
            Map = new Map() { IsShowingUser = true };
            Map.PropertyChanged += Map_PropertyChanged;
        }

        private void Map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapSender = (Map)sender;

            if (mapSender.VisibleRegion != null)
            {
                Position = new Position(mapSender.VisibleRegion.Center.Latitude, mapSender.VisibleRegion.Center.Longitude);
            }
        }
    }
}
