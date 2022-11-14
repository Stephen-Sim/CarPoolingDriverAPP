using CarPoolingDriverAPP.Views.Menu.Vehicle;
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
    }
}
