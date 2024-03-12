namespace WeatherUtilities
{
    class WeatherHelpers
    {
        public static bool willItRain(WeatherForecast w){
            int chance = int.Parse(w.chanceOfPrecepitation);
            return chance>50;
        }
        
    }
}