using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClearSkyPrediction.Model
{
   /*
     Time: This is the start of the forecast period up to the next one
     Total: This is the forecast of total cloud cover at all levels of the atmosphere
     Low: This is the forecast of low cloud cover (stratus) below 5,000ft
     Med: This is the forecast of middle cloud cover (alto) above 5,000ft and below 20,000ft
     High: This is the forecast of high cloud cover (cirrus) above 20,000ft
     Temp: This is the forecast dry bulb temperature at 2 metres above ground
     Dew: This is the forecast wet bulb temperature (or dew point) at 2 metres above ground
     Surface: This is the forecast average windspeed at 10 metres above ground
     30,000ft: This is the forecast average windspeed at 30,000ft above ground
     Seeing : This calculation uses the total cloud cover along with turbulence in 
           the atmosphere and low level wind speed to give an index from 0 to 10 where
           0 is worst and 10 is best seeing conditions. (experimental)
     Transp.: This calculation uses the total amount of water in the atmosphere above 
           your location. It shows the relative humidity in the column of air from 0 
           to 30,000ft and gives an index from 0 to 10 where 0 is worst and 10 is best 
           seeing conditions.(experimental) 
     Pickering: This calculation uses the amount of low and mid level turbulence 
           above your location as well as calculating differences in wind speed and 
           temperature at various levels in the atmosphere to show how much distortion 
           the light rays will experience between 0 and 30,000ft and gives an index 
           from 0 to 10 where 0 is worst and 10 is best seeing conditions.(experimental) 
     RainRisk: This shows the chance of rainfall at your location.
     Weather: This is a combination of all elements displayed as a weather icon   
  */

   public class ForecastStep
   {
      [JsonProperty("temperature")]
      public int Temperature { get; set; }

      [JsonProperty("dewpoint")]
      public int Dewpoint { get; set; }

      [JsonProperty("rain")]
      public double Rain { get; set; }

      [JsonProperty("freezinglevel")]
      public int FreezingLevel { get; set; }

      [JsonProperty("uvIndex")]
      public int UVIndex { get; set; }

      [JsonProperty("totalcloud")]
      public int TotalCloud { get; set; }

      [JsonProperty("lowcloud")]
      public int LowCloud { get; set; }

      [JsonProperty("medcloud")]
      public int MedCloud { get; set; }

      [JsonProperty("highcloud")]
      public int HighCloud { get; set; }

      [JsonProperty("humidity")]
      public int Humidity { get; set; }

      [JsonProperty("windspeed")]
      public int Windspeed { get; set; }

      [JsonProperty("meansealevelpressure")]
      public double MeanSeaLevelPressure { get; set; }

      [JsonProperty("windgustspeed")]
      public int WindGustSpeed { get; set; }

      [JsonProperty("winddirection")]
      public double WindDirection { get; set; }

      [JsonProperty("windletter")]
      public string WindDirectionLetter { get; set; }

      [JsonProperty("icon")]
      public string Icon { get; set; }

      [JsonProperty("iconName")]
      public string IconName { get; set; }

      [JsonProperty("chanceofrain")]
      public int ChanceOfRain { get; set; }

      [JsonProperty("chanceofsnow")]
      public int ChanceOfSnow { get; set; }

      [JsonProperty("dayOfWeek")]
      public int DayOfWeek { get; set; }

      [JsonProperty("weekday")]
      public string Weekday { get; set; }

      [JsonProperty("sunrise")]
      public string Sunrise { get; set; }   // "5:39"

      [JsonProperty("sunset")]
      public string Sunset { get; set; } // "17:46" 

      [Description("This calculation uses the total cloud cover along with turbulence in the atmosphere and low level wind speed to give an index from 0 to 10 where 0 is worst and 10 is best seeing conditions. (experimental)")]
      [JsonProperty("seeingIndex")]
      public int SeeingIndex { get; set; }

      [Description("This calculation uses the amount of low and mid level turbulence above your location as well as calculating differences in wind speed and temperature at various levels in the atmosphere to show how much distortion the light rays will experience between 0 and 30,000ft and gives an index from 0 to 10 where 0 is worst and 10 is best seeing conditions.(experimental) ")]
      [JsonProperty("pickeringIndex")]
      public int PickeringIndex { get; set; }

      [Description("This calculation uses the total amount of water in the atmosphere above your location. It shows the relative humidity in the column of air from 0 to 30,000ft and gives an index from 0 to 10 where 0 is worst and 10 is best seeing conditions.(experimental) ")]
      [JsonProperty("transIndex")]
      public int TransIndex { get; set; }

      [JsonProperty("dayOrNight")]
      public string DayOrNight { get; set; } // D or N

      [JsonProperty("utcTime")]
      public DateTime UTCTime { get; set; } //  "2017-11-01T15:00:00.00"
   }


   public class ForecastLocation
   {
      public List<ForecastStep> ForeCast { get; set; }
      public string Continent { get; set; }
      public string Country { get; set; }
      public string Location { get; set; }
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public int Timezone { get; set; }
   }

   public class MetCheckData
   {
      public ForecastLocation forecastLocation { get; set; }
   }

   public class Forecast
   {
      public MetCheckData metcheckData { get; set; }
      public DateTime feedCreation { get; set; }
      public string feedCreator { get; set; }
      public string feedModel { get; set; }
      public string feedModelRun { get; set; }
      public DateTime feedModelRunInitialTime { get; set; }
      public string feedResolution { get; set; }
   }
}
