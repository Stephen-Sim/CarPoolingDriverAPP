using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CarPoolingDriverAPP.ViewModels.Trip
{
    public class TripPageViewModel : BindableObject
    {
        private List<TripRequest> requests;

        public List<TripRequest> Requests
        {
            get { return requests; }
            set { requests = value; OnPropertyChanged(); }
        }

        public TripService requestService { get; set; }

        public TripPageViewModel()
        {
            Requests = new List<TripRequest>();
            requestService = new TripService();
            this.LoadData();
        }

        private async void LoadData()
        {
            var res = await requestService.GetTrips();
            if (res != null)
            {
                Requests = res;
            }
        }

        private bool isRefreshing = false;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }


        public ICommand ListViewRereshed
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    this.LoadData();
                    IsRefreshing = false;
                });
            }
        }
    }
}
