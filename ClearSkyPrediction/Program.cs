using ClearSkyPrediction.Model;
using ClearSkyPrediction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearSkyPrediction
{
    class Program
    {
      static void Main(string[] args)
      {
         MetCheckService service = new MetCheckService();
         if (args.Length != 2) {
            Console.WriteLine("Oops, you didn't tell me where you are.");
            Console.WriteLine("Please pass the latitude and longitude for you prediction as command line arguments 1 and 2");
            Console.WriteLine("eg. ClearSkyPrediction 52.7 -1.4");

         }
         else {
            try {
               double latitude = double.Parse(args[0]);
               double longitude = double.Parse(args[1]);

              
               Console.WriteLine($"Clear Sky Prediction for the next 10 days at lat/long {latitude}/{longitude} on {DateTime.Now}");
               Console.WriteLine("");

               var getListTask = service.GetForecast(latitude, longitude);
               Task.WaitAll(getListTask); // block while the task completes

               Forecast forecast = getListTask.Result;
               IEnumerable<ForecastStep> possibleSlots = forecast.metcheckData.forecastLocation.ForeCast.Where(s => s.SeeingIndex >= 7 && s.DayOrNight == "N");
               int currentDay = -1;
               if (possibleSlots.Any()) {
                  foreach (ForecastStep step in possibleSlots) {
                     if (step.UTCTime.DayOfYear != currentDay) {
                        if (currentDay != -1) {
                           Console.WriteLine();
                        }
                        currentDay = step.UTCTime.DayOfYear;
                        Console.WriteLine($"{step.Weekday} {step.UTCTime.Date}");
                        Console.WriteLine($"     {step.UTCTime.TimeOfDay} - Seeing:{step.SeeingIndex}, Transp:{step.TransIndex}, Pickering:{step.PickeringIndex}, Temp: {step.Temperature}, Dew:{step.Dewpoint}, Rain = {step.Rain}%");
                     }
                     else {
                        Console.WriteLine($"     {step.UTCTime.TimeOfDay} - Seeing:{step.SeeingIndex}, Transp:{step.TransIndex}, Pickering:{step.PickeringIndex}, Temp: {step.Temperature}, Dew:{step.Dewpoint}, Rain = {step.Rain}%");
                     }
                  }
                  Console.WriteLine("");
                  Console.WriteLine("Seeing, Transp and Pickering range from 0-10 (worst to best), temp and dew in °C");
               }
               else {
                  Console.WriteLine("Time to rework some earlier data or get on with the decorating.");
               }
               Console.WriteLine("");
               Console.WriteLine("All weather data supplied by MetCheck. Please visit: ");
               Console.WriteLine(@"    http://www.metcheck.com/OTHER/ghx_global_hybrid_model.asp");
            }
            catch (Exception ex) {
               Console.WriteLine("");
               Console.WriteLine("Oops! Something went wrong, here's a clue:");
               Console.WriteLine(ex.Message);
            }
         }
         Console.WriteLine("");
         Console.WriteLine("Clear Sky Predition by Lunatic Software. email phil@unitysoftware.co.uk");
         Console.WriteLine("Press any key to exit.");
         Console.ReadKey();
      }
   }
}
