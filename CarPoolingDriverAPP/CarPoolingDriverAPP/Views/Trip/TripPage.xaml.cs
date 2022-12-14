using CarPoolingDriverAPP.ViewModels.Trip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarPoolingDriverAPP.Views.Trip
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripPage : ContentPage
    {
        public TripPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.BindingContext = new TripPageViewModel();
        }
    }
}