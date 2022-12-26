using CarPoolingDriverAPP.Views.Auth;
using CarPoolingDriverAPP.Views.Home;
using CarPoolingDriverAPP.Views.Menu;
using CarPoolingDriverAPP.Views.Menu.Vehicle;
using CarPoolingDriverAPP.Views.Trip;
using CarPoolingDriverAPP.Views.Trip.Chat;
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

            // Home
            Routing.RegisterRoute($"Home/{nameof(PinStartLocationPage)}", typeof(PinStartLocationPage));
            Routing.RegisterRoute($"Home/{nameof(PinEndLocationPage)}", typeof(PinEndLocationPage));
            Routing.RegisterRoute($"Home/{nameof(ConfirmTripPage)}", typeof(ConfirmTripPage));

            // Trip
            Routing.RegisterRoute($"Trip/{nameof(SearchingTripPage)}", typeof(SearchingTripPage));
            Routing.RegisterRoute($"Trip/{nameof(CanceledTripPage)}", typeof(CanceledTripPage));
            Routing.RegisterRoute($"Trip/{nameof(PendingTripPage)}", typeof(PendingTripPage));
            Routing.RegisterRoute($"Trip/{nameof(CompletedTripPage)}", typeof(CompletedTripPage));

            // Chat
            Routing.RegisterRoute($"Chat/{nameof(ChatPassengerPage)}", typeof(ChatPassengerPage));

            // Menu
            Routing.RegisterRoute($"Menu/{nameof(EditProfilePage)}", typeof(EditProfilePage));
            Routing.RegisterRoute($"Menu/{nameof(ChangePasswordPage)}", typeof(ChangePasswordPage));
            Routing.RegisterRoute($"Menu/{nameof(AboutTheAppPage)}", typeof(AboutTheAppPage));
            Routing.RegisterRoute($"Menu/Vehicle/{nameof(VehicleInformationPage)}", typeof(VehicleInformationPage));
            Routing.RegisterRoute($"Menu/Vehicle/{nameof(AddOrEditVehiclePage)}", typeof(AddOrEditVehiclePage));

        }
    }
}