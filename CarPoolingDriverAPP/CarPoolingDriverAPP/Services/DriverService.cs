using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CarPoolingDriverAPP.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CarPoolingDriverAPP.Services
{
    public class DriverService
    {
        private string url = $"{APIConstant.APIURL}Driver/";
        public HttpClient client { get; set; }

        public DriverService()
        {
            client = new HttpClient();
            var token = App.Current.Properties["token"] as string;
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("token", token);
        }

        public async Task<DriverInfo> GetDriverInfo()
        {
            try
            {
                string url = $"{this.url}GetDriverInfo";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<DriverInfo>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public async Task<List<StatData>> GetStatInfo(int schecduleId)
        {
            try
            {
                string url = $"{this.url}GetStatInfo?schecduleId={schecduleId}";
                var res = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<List<StatData>>(res);
                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
    }
}
