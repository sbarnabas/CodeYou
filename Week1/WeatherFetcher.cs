using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using WeatherUtilities.WeatherTypes;

namespace WeatherUtilities
{



    /// <summary>
    /// This class encapsulates the logic necessary to fetch forecasts from the weather.gov api.
    /// The weather.gov api provides forecasts given a station code and grid coordinates; conveniently, it also provides an api to get those from lat and long
    /// Since most people won't know their latitude and longitude, we need to "geocode", or translate from addresses to lat/long.
    /// 
    /// </summary>
    public class WeatherFetcher
    {
        record GeoPoint(double longitude, double latitude);
        static Dictionary<string, GeoPoint> zipCodeLookup = parseZipData();
        Dictionary<string, PointProperties?> cachedForecastProps = new Dictionary<string, PointProperties?>();


        readonly Uri baseUri = new Uri("https://api.weather.gov/");

        public WeatherFetcher()
        {
        }


        public WeatherForecast getCurrentWeatherForZip(string zip){
            return getForecastDataForZip(zip).First();
        }

        public List<WeatherForecast> getForecastDataForZip(string zip)
        {
            if (!zipCodeLookup.ContainsKey(zip))
            {
                throw new ArgumentException($"Invalid ZipCode {zip}");
            }
            //populate uri cache if we don't have it yet
            if (!cachedForecastProps.ContainsKey(zip))
            {
                var path = getForecastPathForZipAsync(zip);
                path.Wait();
                if (path.Result != null)
                {
                    cachedForecastProps.Add(zip, path.Result);
                }
                else throw new ArgumentException($"Unable to get forecast for {zip}");
            }
            using (HttpClient client = new HttpClient())
            {
                //weather.gov requires a user-agent
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; codeYouDemo/1.0)");
                var forecastUrl = cachedForecastProps[zip]?.forecast;
                var city = cachedForecastProps[zip]?.relativeLocation?.properties?.city;
                var state = cachedForecastProps[zip]?.relativeLocation?.properties?.state;
                if (forecastUrl != null)
                {

                    var result = client.GetFromJsonAsync<RawForecastData>(forecastUrl);
                    result.Wait();

                    return result.Result?.properties?.periods?.Select(p => new WeatherForecast
                    {
                        when = p.name ?? "Unknown",
                        shortForecast = p.shortForecast ?? "none",
                        longForecast = p.detailedForecast ?? "none",
                        chanceOfPrecepitation =
                            String.IsNullOrWhiteSpace(p.probabilityOfPrecipitation?.value.ToString()) ? "0" :
                            p.probabilityOfPrecipitation?.value.ToString()??"0",
                        humidity = p.relativeHumidity?.value.ToString()??"0",
                        isDaytime = p.isDaytime,
                        temp = p.temperature,
                        city = city ?? "unknown",
                        state = state ?? "unknown",
                        zip = zip

                    }).ToList() ?? new List<WeatherForecast>();


                }
                else throw new ArgumentException($"Unable to get forecast for {zip}");
            }

        }
        async Task<PointProperties?> getForecastPathForZipAsync(string zip)
        {
            Uri? pointLookup;
            Uri.TryCreate(baseUri, $"/points/{formatGeoPoint(zipCodeLookup[zip])}", out pointLookup);
            if (pointLookup != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    //weather.gov requires a user-agent
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; codeYouDemo/1.0)");

                    var result = await client.GetFromJsonAsync<RawPointData>(pointLookup.ToString());
                    return result?.properties;
                }
            }
            return null;
        }
        private string formatGeoPoint(GeoPoint g)
        {
            return $"{g.latitude},{g.longitude}";
        }
        private static Dictionary<string, GeoPoint> parseZipData(string fileName = "zipData/us_zip_codes_to_longitude_and_latitude.csv")
        {
            Dictionary<string, GeoPoint> zipData = new Dictionary<string, GeoPoint>();
            using (StreamReader sr = File.OpenText(fileName))
            {
                //in real code, this would probably cause *tons* of bugs down the road.
                //add checking for invalid values, format changes, files not existing, etc.
                //There is already an off-by-one error that cancels out(!) - don't do stuff like this in production!

                string? s = sr.ReadLine(); //first line is header info
                //format is "geopoint","Daylight_savings_time_flag","Timezone","Longitude","Latitude","State","City","Zip"
                while ((s = sr.ReadLine()) != null)
                {
                    var fields = s.Split(',');
                    zipData.Add(fields[8], new GeoPoint(double.Parse(fields[4]), double.Parse(fields[5])));
                }

            }
            return zipData;

        }

    }

}