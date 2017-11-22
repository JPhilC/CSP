﻿using ClearSkyPrediction.Model;
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
         MetCheckService mcService = new MetCheckService();
         USNOApiService usnoService = new USNOApiService();
         if (args.Length != 2) {
            Console.WriteLine("Oops, you didn't tell me where you are.");
            Console.WriteLine("Please pass the latitude and longitude for you prediction as command line arguments 1 and 2");
            Console.WriteLine("eg. ClearSkyPrediction 52.7 -1.4");

         }
         else {
            try {
               double latitude = double.Parse(args[0]);
               double longitude = double.Parse(args[1]);
              
               Console.WriteLine($"Clear Sky Prediction for the next 10 days at lat/long {latitude}/{longitude} on {DateTime.Now.Date:dd/MM/yyyy}");
               Console.WriteLine("");

               var getListTask = mcService.GetForecast(latitude, longitude);
               DateTime midnight = DateTime.Now.AddDays(1).Date;
               var getSunAndMoonDataTask = usnoService.GetSunAndMoonData(latitude, longitude, 0, midnight);
               Task.WaitAll(getSunAndMoonDataTask, getListTask ); // block while the task completes

               Forecast forecast = getListTask.Result;
               SunAndMoonForecast sunAndMoonForecast = getSunAndMoonDataTask.Result;
               IEnumerable<ForecastStep> possibleSlots = forecast.metcheckData.forecastLocation.ForeCast.Where(s => s.SeeingIndex >= 7 && s.DayOrNight == "N");
               int currentDay = -1;
               DateTime eveningOf;
               if (possibleSlots.Any()) {
                  foreach (ForecastStep step in possibleSlots) {
                     if (step.UTCTime.Hour > 12) {
                        eveningOf = step.UTCTime.Date;
                     }
                     else {
                        eveningOf = step.UTCTime.Date.AddDays(-1);
                     }

                     if (eveningOf.DayOfYear != currentDay) {
                        if (currentDay != -1) {
                           Console.WriteLine();
                        }
                        currentDay = eveningOf.DayOfYear;
                        SunAndMoonData moonData = sunAndMoonForecast.Forecast.Where(day => day.Key == eveningOf.Date).Select(day=>day.Value).FirstOrDefault();
                        if (moonData != null) {
                           string moonRises = moonData.MoonData.Where(p => p.Code == "R").Select(p => p.Time).FirstOrDefault();
                           if (moonData.Fracillum != null) {
                              Console.WriteLine($"Evening of {step.Weekday} {eveningOf.Date:dd/MM/yyyy} Moon: {moonData.Fracillum} {moonData.CurrentPhase}" + (moonRises!=null?$" rises: {moonRises}":""));
                           }
                           else {
                              Console.WriteLine($"Evening of {step.Weekday} {eveningOf.Date:dd/MM/yyyy} Moon: {moonData.ClosesPhase.Phase}" + (moonRises != null ? $" rises: {moonRises}" : ""));
                           }
                        }
                        else {
                           Console.WriteLine($"Evening of {step.Weekday} {eveningOf.Date:dd/MM/yyyy}");
                        }
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
               Console.WriteLine("Lunar data courtesy of US Navy Observatory . Please visit: ");
               Console.WriteLine(@"    http://aa.usno.navy.mil/data/docs/api.php");
            }
            catch (Exception ex) {
               Console.WriteLine("");
               Console.WriteLine("Oops! Something went wrong, here's a clue:");
               Console.WriteLine(ex.Message);
            }
         }
         Console.WriteLine("");
         Console.WriteLine("Clear Sky Prediction by Lunatic Software. email phil@lunaticsoftware.org");
         Console.WriteLine("Press any key to exit.");
         Console.ReadKey();
      }
   }
}
