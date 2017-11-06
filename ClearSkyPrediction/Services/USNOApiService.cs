using ClearSkyPrediction.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSkyPrediction.Services
{
   public class USNOApiService
   {
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public int TimeZoneOffset { get; set; }

      public DateTime DateOfInterest { get; set; }

      private SunAndMoonForecast forecast = new SunAndMoonForecast();

      public async Task<SunAndMoonForecast> GetSunAndMoonData(double latitude, double longitude, int timeZoneOffset, DateTime dateOfInterest)
      {
         Latitude = latitude;
         Longitude = longitude;
         TimeZoneOffset = timeZoneOffset;
         DateOfInterest = dateOfInterest;
         return await GetSunAndMoonData();
      }

      public async Task<SunAndMoonForecast> GetSunAndMoonData()
      {
         forecast.Forecast.Clear();
         Task[] tasks = new Task[10];
         // http://api.usno.navy.mil/rstt/oneday?date=11/11/2017&coords=52.60,-1.33&tz=0
         for (int i = 0; i < 10; i++) {
            tasks[i] = GetSingleDay(DateOfInterest.AddDays(i).Date);
         }
         await Task.WhenAll(tasks);
         return forecast;
      }

      private async Task GetSingleDay(DateTime forecastDay)
      {
         string uriString = $"http://api.usno.navy.mil/rstt/oneday?date={forecastDay:MM/dd/yyyy}&coords={this.Latitude},{this.Longitude}&tz={this.TimeZoneOffset}";
         Uri geturi = new Uri(uriString); //replace your url  
         System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
         System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi);
         if (responseGet.IsSuccessStatusCode) {
            string data = await responseGet.Content.ReadAsStringAsync();
            SunAndMoonData dayForecast = JsonConvert.DeserializeObject<SunAndMoonData>(data,
               new IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal });
            lock (forecast) {
               forecast.Forecast.Add(forecastDay, dayForecast);
            }
         }
      }

   }
}
