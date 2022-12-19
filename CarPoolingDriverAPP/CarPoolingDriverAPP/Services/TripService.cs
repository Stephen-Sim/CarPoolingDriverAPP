using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CarPoolingDriverAPP.Models;

namespace CarPoolingDriverAPP.Services
{
    public class TripService
    {
        private string url = $"{APIConstant.APIURL}trip/";
        public HttpClient client { get; set; }

        public TripService()
        {
            client = new HttpClient();
            var token = App.Current.Properties["token"] as string;
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("token", token);
        }

        public async Task<List<TripRequest>> GetTrips()
        {
            try
            {
                string url = $"{this.url}GetTrips";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<List<TripRequest>>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<bool> CreateTrip(TripRequest request)
        {
            try
            {
                string url = $"{this.url}Create";
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync(url, content);

                if (res.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }
    }
}
