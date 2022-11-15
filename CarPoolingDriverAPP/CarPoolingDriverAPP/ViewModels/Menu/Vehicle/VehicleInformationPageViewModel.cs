using CarPoolingDriverAPP.Services;
using CarPoolingDriverAPP.Views.Menu.Vehicle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CarPoolingDriverAPP.ViewModels.Menu.Vehicle
{
    public class VehicleInformationPageViewModel : BindableObject
    {
        public ICommand AddNewVehicleClicked
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"Menu/Vehicle/{nameof(AddOrEditVehiclePage)}");
                });
            }
        }

        public ICommand EditButtonClicked
        {
            get
            {
                return new Command(async param =>
                {
                    var vehicle = JsonConvert.SerializeObject(param);
                    await Shell.Current.GoToAsync($"Menu/Vehicle/{nameof(AddOrEditVehiclePage)}?ExistVehicle={vehicle}");
                });
            }
        }

        public VehicleService vehicleService { get; set; }
        public VehicleInformationPageViewModel()
        {
            vehicleService = new VehicleService();

            this.GetVehicles();
        }

        private List<Models.Vehicle> vehicles;

        public List<Models.Vehicle> Vehicles
        {
            get { return vehicles; }
            set { vehicles = value; OnPropertyChanged(); }
        }

        private async void GetVehicles()
        {
            var result = await vehicleService.GetVehicles();
            if (result != null)
            {
                Vehicles = result;
            }
        }
    }
}
