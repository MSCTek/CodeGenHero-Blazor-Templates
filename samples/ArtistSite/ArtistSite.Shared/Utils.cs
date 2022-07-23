using System;
using System.Linq;

namespace ArtistSite.Shared
{
    public class Utils
    {
        private Random _random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string retVal = new string(Enumerable.Repeat(chars, length).Select(x => x[_random.Next(x.Length)]).ToArray());

            return retVal;
        }

        //        private static readonly string[] Summaries = new[]
        //{
        //            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //        };

        //public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        //{
        //    var rng = new Random();
        //    return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = startDate.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    }).ToArray());
        //}
    }
}