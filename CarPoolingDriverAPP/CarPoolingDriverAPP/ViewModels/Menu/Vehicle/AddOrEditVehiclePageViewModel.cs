using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CarPoolingDriverAPP.ViewModels.Menu.Vehicle
{
    [QueryProperty(nameof(ExistVehicle), nameof(ExistVehicle))]
    public class AddOrEditVehiclePageViewModel : BindableObject
    {
        private string existVehicle = "";

        public string ExistVehicle
        {
            get => existVehicle;
            set 
            {
                IsEdit = true; CreteOrEditButtonText = "Edit";
                existVehicle = Uri.UnescapeDataString(value ?? string.Empty);
                this.LoadVehicle(existVehicle);
            }
        }

        private string creteOrEditButtonText = "Create";

        public string CreteOrEditButtonText
        {
            get { return creteOrEditButtonText; }
            set { creteOrEditButtonText = value; OnPropertyChanged(); }
        }


        public void LoadVehicle(string content)
        {
            var vehicle = JsonConvert.DeserializeObject<Models.Vehicle>(content);
            Vehicle = vehicle;
        }

        private bool isEdit = false;

        public bool IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; OnPropertyChanged(); }
        }

        public Models.Vehicle vehicle { get; set; }
        public Models.Vehicle Vehicle { get { return vehicle; } set { vehicle = value; OnPropertyChanged(); } }

        public AddOrEditVehiclePageViewModel()
        {
            Vehicle = new Models.Vehicle();
            vehicleService = new VehicleService();
        }

        public ICommand CancelButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.Navigation.PopAsync();
                });
            }
        }

        public VehicleService vehicleService { get; set; }

        public ICommand CreateButtonClicked
        {
            get
            {
                return new Command(async () =>
                {
                    if (string.IsNullOrEmpty(Vehicle.Name) || string.IsNullOrEmpty(Vehicle.PlatNo) || string.IsNullOrEmpty(Vehicle.Color) || Vehicle.Capacity <= 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "All the fields are required!!", "Ok");
                        return;
                    }

                    var res = IsEdit ? await vehicleService.EditVehicle(Vehicle) : await vehicleService.CreateVehicle(Vehicle);

                    if (res)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "The Vehicle is successfully " + (isEdit ? "edited" : "created") + "!!", "Ok");
                        await Shell.Current.Navigation.PopAsync();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Operation Failed!!", "Ok");
                    }
                });
            }
        }

        public ICommand DeleteButtonClicked
        {
            get
            {
                return new Command(async (param) =>
                {
                    var dialogResult = await App.Current.MainPage.DisplayAlert("Alert", "Are you sure to Delete this vehicle?", "Yes", "No");

                    if (dialogResult)
                    {
                        var res = await vehicleService.DeleteVehicle(Vehicle.Id);

                        if (res)
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "The Vehicle is deleted!!", "Ok");
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
