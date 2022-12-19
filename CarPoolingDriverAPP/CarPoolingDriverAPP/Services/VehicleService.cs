using CarPoolingDriverAPP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CarPoolingDriverAPP.Services
{
    public class VehicleService
    {
        private string url = $"{APIConstant.APIURL}Vehicle/";
        public HttpClient client { get; set; }

        public VehicleService()
        {
            client = new HttpClient();
            var token = App.Current.Properties["token"] as string;
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("token", token);
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            try
            {
                string url = $"{this.url}GetVehicles";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<List<Vehicle>>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<bool> CreateVehicle(Vehicle vehicle)
        {
            try
            {
                string url = $"{this.url}Create";
                var json = JsonConvert.SerializeObject(vehicle);
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

        public async Task<bool> EditVehicle(Vehicle vehicle)
        {
            try
            {
                string url = $"{this.url}Edit";
                var json = JsonConvert.SerializeObject(vehicle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PutAsync(url, content);

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

        public async Task<bool> DeleteVehicle(int vehicleID)
        {
            try
            {
                string url = $"{this.url}Delete";

                var dict = new Dictionary<string, string>
                {
                    { "vehicleId", vehicleID.ToString() }
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
    }
}
