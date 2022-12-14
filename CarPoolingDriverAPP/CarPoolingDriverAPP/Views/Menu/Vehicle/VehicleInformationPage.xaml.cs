using CarPoolingDriverAPP.ViewModels.Menu.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarPoolingDriverAPP.Views.Menu.Vehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleInformationPage : ContentPage
    {
        public VehicleInformationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.BindingContext = new VehicleInformationPageViewModel();
        }
    }
}