using System.Text.Json.Serialization;

//this file contains type for deserializing weather data from weather.gov
//see the geojson spec for more info
// https://geojson.org/geojson-ld/geojson-context.jsonld
// Very hacky and only done for deserializing from weather.gov; not meant to be a general purpose GeoJson impl
namespace WeatherUtilities.WeatherTypes {
public class Dewpoint
{
    public string? unitCode { get; set; }
    public double value { get; set; }
}

public class Elevation
{
    public string? unitCode { get; set; }
    public double value { get; set; }
}

public class Geometry
{
    public string? type { get; set; }
    public List<double>? coordinates { get; set; }
}


public class ForecastGeometry
{
    public string? type { get; set; }
    public List<List<List<double>>>? coordinates { get; set; }
}

public class Period
{
    public int number { get; set; }
    public string? name { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public bool isDaytime { get; set; }
    public int temperature { get; set; }
    public string? temperatureUnit { get; set; }
    public object? temperatureTrend { get; set; }
    public ProbabilityOfPrecipitation? probabilityOfPrecipitation { get; set; }
    public Dewpoint? dewpoint { get; set; }
    public RelativeHumidity? relativeHumidity { get; set; }
    public string? windSpeed { get; set; }
    public string? windDirection { get; set; }
    public string? icon { get; set; }
    public string? shortForecast { get; set; }
    public string? detailedForecast { get; set; }
}

public class ProbabilityOfPrecipitation
{
    public string? unitCode { get; set; }
    public int? value { get; set; }
}

public class Properties
{
    public DateTime updated { get; set; }
    public string? units { get; set; }
    public string? forecastGenerator { get; set; }
    public DateTime generatedAt { get; set; }
    public DateTime updateTime { get; set; }
    public string? validTimes { get; set; }
    public Elevation? elevation { get; set; }
    public List<Period>? periods { get; set; }
}

public class RelativeHumidity
{
    public string? unitCode { get; set; }
    public int value { get; set; }
}

public class RawForecastData
{
    [JsonPropertyName("@context")]
    public List<object>? context { get; set; }
    public string? type { get; set; }
    public ForecastGeometry? geometry { get; set; }
    public Properties? properties { get; set; }
}

public class Bearing
{
    public string? unitCode { get; set; }
    public int value { get; set; }
}

public class Distance
{
    public string? unitCode { get; set; }
    public double value { get; set; }
}

public class PointProperties
{
    [JsonPropertyName("@id")]
    public string? id { get; set; }

    [JsonPropertyName("@type")]
    public string? type { get; set; }
    public string? cwa { get; set; }
    public string? forecastOffice { get; set; }
    public string? gridId { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }
    public string? forecast { get; set; }
    public string? forecastHourly { get; set; }
    public string? forecastGridData { get; set; }
    public string? observationStations { get; set; }
    public RelativeLocation? relativeLocation { get; set; }
    public string? forecastZone { get; set; }
    public string? county { get; set; }
    public string? fireWeatherZone { get; set; }
    public string? timeZone { get; set; }
    public string? radarStation { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public Distance? distance { get; set; }
    public Bearing? bearing { get; set; }
}

public class RelativeLocation
{
    public string? type { get; set; }
    public Geometry? geometry { get; set; }
    public PointProperties? properties { get; set; }
}

public class RawPointData
{
    [JsonPropertyName("@context")]
    public List<object>? context { get; set; }
    public string? id { get; set; }
    public string? type { get; set; }
    public Geometry? geometry { get; set; }
    public PointProperties? properties { get; set; }
}

}