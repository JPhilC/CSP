using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearSkyPrediction.Model
{
   public class SunAndMoonForecast
   {
      public Dictionary<DateTime, SunAndMoonData> Forecast { get; set; }

      public SunAndMoonForecast()
      {
         Forecast = new Dictionary<DateTime, SunAndMoonData>();
      }
   }

   public class SunAndMoonData
   {
      [JsonProperty("error")] //: false,
      public string Error { get; set; }
      [JsonProperty("apiversion")]    //  "2.0.0",
      public string APIVersion { get; set; }
      [JsonProperty("year")]    //  2017,
      public int Year { get; set; }
      [JsonProperty("month")]    //  11,
      public int Month { get; set; }
      [JsonProperty("day")]    //  11,
      public int Day { get; set; }
      [JsonProperty("dayofweek")]    //  "Saturday",
      public string DayOfWeek { get; set; }
      [JsonProperty("datechanged")]    //  false,
      public bool DateChanged { get; set; }
      [JsonProperty("lon")]    //  -1.33,
      public double Longitude { get; set; }
      [JsonProperty("lat")]    //  52.6,
      public double Latitude { get; set; }
      [JsonProperty("tz")]    //  0,
      public int TimeZone { get; set; }

      [JsonProperty("sundata")]
      public List<Phenomenon> SunData { get; set; }

      [JsonProperty("moondata")]    //  [
      public List<Phenomenon> MoonData { get; set; }

      [JsonProperty("prevmoondata")]    //  [
      public List<Phenomenon> PreviousMoonData { get; set; }

      [JsonProperty("closestphase")]    //  {
      public ClosesPhase ClosesPhase { get; set; }

      [JsonProperty("fracillum")]    //  "43%",
      public string Fracillum { get; set; }

      [JsonProperty("curphase")]    // "Waning Crescent"
      public string CurrentPhase { get; set; }
   }


   public class ClosesPhase
   {
      [JsonProperty("phase")]
      public string Phase { get; set; }

      [JsonProperty("date")]
      public string Date { get; set; }

      [JsonProperty("time")]
      public string Time { get; set; }
   }

   public class Phenomenon
   {
      [JsonProperty("phen")]
      public string Code { get; set; }

      [JsonProperty("time")]
      public string Time { get; set; }
   }
}
