using CarPoolingDriverAPP.Models;
using CarPoolingDriverAPP.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace CarPoolingDriverAPP.ViewModels.Menu
{
    public class StatsPageViewModel : BindableObject
    {
        public DriverService driverService { get; set; }

        private DriverInfo driverInfo;

        public DriverInfo DriverInfo
        {
            get { return driverInfo; }
            set { driverInfo = value; OnPropertyChanged(); }
        }

        private Chart statChart;

        public Chart StatChart
        {
            get { return statChart; }
            set { statChart = value; OnPropertyChanged(); }
        }


        private ImageSource profileImage = null;

        public List<Schedule> ScheduleList { get; set; }

        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set { profileImage = value; OnPropertyChanged(); }
        }

        private Schedule selectedSchedule;

        public Schedule SelectedSchedule
        {
            get { return selectedSchedule; }
            set
            {
                if (selectedSchedule != value)
                {
                    selectedSchedule = value;
                    this.LoadChart();
                }
            }
        }

        private string totalEarned = "RM 0.00";

        public string TotalEarned
        {
            get { return totalEarned; }
            set { totalEarned = value; OnPropertyChanged(); }
        }

        private string totalPassengers = "0";

        public string TotalPassengers
        {
            get { return totalPassengers; }
            set { totalPassengers = value; OnPropertyChanged(); }
        }

        private async void LoadChart()
        {
            var res = await driverService.GetStatInfo(SelectedSchedule.Id);

            if (res != null)
            {
                var chartEntry = new List<ChartEntry>();
                var count = 0;
                var total = 0.0m;

                if (SelectedSchedule.Id == 1)
                {
                    for (int i = 0; i < res.Count; i++)
                    {
                        chartEntry.Add(new ChartEntry((float)res[i].TotalEarned)
                        {
                            Label = res[i].DayOfWeek,
                            ValueLabel = res[i].TotalEarned.ToString(),
                            Color = SKColor.Parse("#F6BE00")
                        });

                        count += res[i].TotalPassengers;
                        total += res[i].TotalEarned;
                    }
                    StatChart = new LineChart();
                }
                else if (SelectedSchedule.Id == 2)
                {
                    for (int i = 0; i < res.Count; i++)
                    {
                        chartEntry.Add(new ChartEntry((float)res[i].TotalEarned)
                        {
                            Label = res[i].Month,
                            ValueLabel = res[i].TotalEarned.ToString(),
                            Color = SKColor.Parse("#F6BE00")
                        });

                        count += res[i].TotalPassengers;
                        total += res[i].TotalEarned;
                    }

                    StatChart = new BarChart();
                }
                else if (SelectedSchedule.Id == 3)
                {
                    for (int i = 0; i < res.Count; i++)
                    {
                        chartEntry.Add(new ChartEntry((float)res[i].TotalEarned)
                        {
                            Label = res[i].Year,
                            ValueLabel = res[i].TotalEarned.ToString(),
                            Color = SKColor.Parse("#F6BE00")
                        });

                        count += res[i].TotalPassengers;
                        total += res[i].TotalEarned;
                    }

                    StatChart = new BarChart();
                }

                TotalPassengers = count.ToString();
                TotalEarned = "RM " + total.ToString("0.00");
                StatChart.Entries = chartEntry;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "There is no data available yet!!!", "Ok");
            }
        }

        public StatsPageViewModel()
        {
            ScheduleList = new List<Schedule>();
            ScheduleList.Add(new Schedule { Id = 1, Name = "Last 7 Days"});
            ScheduleList.Add(new Schedule { Id = 2, Name = "This Year" });
            ScheduleList.Add(new Schedule { Id = 3, Name = "All the Years" });

            DriverInfo = new DriverInfo();
            driverService = new DriverService();
            StatChart = new BarChart();
            this.LoadDriverInfo();
        }

        private async void LoadDriverInfo()
        {
            var res = await driverService.GetDriverInfo();
            if (res != null)
            {
                DriverInfo = res;

                if (DriverInfo.DriverProfile != null)
                {
                    Stream stream = new MemoryStream(DriverInfo.DriverProfile);
                    ProfileImage = ImageSource.FromStream(() => stream);
                }
            }
        }
    }
}
