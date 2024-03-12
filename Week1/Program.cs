using System;
using System.Diagnostics;
using WeatherUtilities;
internal class Program
{
    private static  void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        WeatherUtilities.WeatherFetcher w= new WeatherUtilities.WeatherFetcher();
        w.getForecastDataForZip("45069");
        
    }
}