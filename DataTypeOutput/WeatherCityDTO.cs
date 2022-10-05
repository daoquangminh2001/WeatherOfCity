namespace WeatherOfCity.DataTypeOutput
{
    public class WeatherCityDTO
    {
        public string City_Name { get; set; }
        public DateTime? Time { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
