using System;
using System.Diagnostics;
using WeatherUtilities;
internal class Program
{
    private static  void Main(string[] args)
    {
        WeatherUtilities.WeatherFetcher w= new WeatherUtilities.WeatherFetcher();
        WeatherForecast forecast = w.getCurrentWeatherForZip("40018");
        Debug.WriteLine(forecast.ToString());
        if(forecast.isDaytime && forecast.temp > 60) {
            Console.WriteLine("It's warm outside!");
        }
        else if (!forecast.isDaytime) {
            Console.WriteLine("It's dark out!");
        }
        else 
        {
            Console.WriteLine(forecast.longForecast);
        }
        if(WeatherHelpers.willItRain(w)){
            Console.WriteLine("It looks like it will rain.");
        }
        
    }
}