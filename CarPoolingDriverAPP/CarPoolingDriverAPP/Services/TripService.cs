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
        private string url = $"{APIConstant.APIURL}Trip/";
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

        public async Task<TripRequest> GetTrip(int id)
        {
            try
            {
                string url = $"{this.url}GetTrip?tripId={id}";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<TripRequest>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<List<RequestRequest>> GetTripsRequest(int id)
        {
            try
            {
                string url = $"{this.url}GetTripsRequestByTrip?tripId={id}";
                var trip = await client.GetAsync(url);

                if (trip.IsSuccessStatusCode)
                {
                    var res = await trip.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<RequestRequest>>(res);
                    return result;
                }

                return null;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        public async Task<bool> CancelTrip(string tripId)
        {
            try
            {
                string url = $"{this.url}CancelTrip";

                var dict = new Dictionary<string, string>
                {
                    { "tripId", tripId }
                };

                var content = new FormUrlEncodedContent(dict);
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

        public async Task<bool> ConfirmTrip(ConfirmTripRequest confirmTripRequest)
        {
            try
            {
                string url = $"{this.url}ConfirmTrip";
                var json = JsonConvert.SerializeObject(confirmTripRequest);
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

        public async Task<List<RequestRequest>> GetAcceptedRequest(int id)
        {
            try
            {
                string url = $"{this.url}GetAcceptedRequest?tripId={id}";
                var request = await client.GetAsync(url);

                if (request.IsSuccessStatusCode)
                {
                    var res = await request.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<RequestRequest>>(res);
                    return result;
                }

                return null;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<string> CompleteTrip(string tripId)
        {
            try
            {
                string url = $"{this.url}CompleteTrip";
                var dict = new Dictionary<string, string>
                {
                    { "tripId", tripId }
                };

                var content = new FormUrlEncodedContent(dict);
                var res = await client.PostAsync(url, content);

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }

                return string.Empty;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return string.Empty;
            }
        }

        public async Task<string> GetCompleteTripInfo(int id)
        {
            try
            {
                string url = $"{this.url}GetCompleteTripInfo?tripId={id}";
                var res = await client.GetStringAsync(url);
                return res;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return string.Empty;
            }
        }
    }
}
