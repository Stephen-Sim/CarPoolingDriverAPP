using CarPoolingDriverAPP.Views.Auth;
using CarPoolingDriverAPP.Views.Menu;
using CarPoolingDriverAPP.Views.Menu.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarPoolingDriverAPP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Auth
            Routing.RegisterRoute($"Auth/{nameof(RegisterPage)}", typeof(RegisterPage));

            // Menu
            Routing.RegisterRoute($"Menu/{nameof(EditProfilePage)}", typeof(EditProfilePage));
            Routing.RegisterRoute($"Menu/{nameof(ChangePasswordPage)}", typeof(ChangePasswordPage));
            Routing.RegisterRoute($"Menu/{nameof(AboutTheAppPage)}", typeof(AboutTheAppPage));
            Routing.RegisterRoute($"Menu/Vehicle/{nameof(VehicleInformationPage)}", typeof(VehicleInformationPage));
            Routing.RegisterRoute($"Menu/Vehicle/{nameof(AddOrEditVehiclePage)}", typeof(AddOrEditVehiclePage));

        }
    }
}