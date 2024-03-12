namespace WeatherUtilities{

    public record WeatherForecast
    {
        public required string when { get;set;}
        public required string shortForecast {get;set;}
        public required string longForecast {get;set;}
        public required string chanceOfPrecepitation {get;set;}
        public required string humidity {get;set;}
        public required bool isDaytime {get;set;}

        public required int temp {get;set;}

        public required string city {get;set;}
        public required string state {get;set;}
        public required string zip {get;set;}
    }

}