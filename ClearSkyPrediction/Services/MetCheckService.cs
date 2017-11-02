using ClearSkyPrediction.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;

namespace ClearSkyPrediction.Services
{
   public class MetCheckService
    {
      public double Latitude { get; set; }
      public double Longitude { get; set; }

      public async Task<Forecast> GetForecast(double latitude, double longitude)
      {
         this.Latitude = latitude;
         this.Longitude = longitude;
         return await GetForecast();
      }

      public async Task<Forecast> GetForecast()
      {
         // http://ws1.metcheck.com/ENGINE/v9_0/json.asp?lat=52.7&lon=-1.4&FCt=As //
         string uriString = $"http://ws1.metcheck.com/ENGINE/v9_0/json.asp?lat={this.Latitude}&lon={this.Longitude}&FCt=As";
         Uri geturi = new Uri(uriString); //replace your url  
         System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
         System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi);
         if (responseGet.IsSuccessStatusCode) {
            string data = await responseGet.Content.ReadAsStringAsync();
            Forecast forecast = JsonConvert.DeserializeObject<Forecast>(data,
               new IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal });
            return forecast;
         }
         return null;
      }
   }
}

