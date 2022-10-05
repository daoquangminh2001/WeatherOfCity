using System.Collections.Generic;
using WeatherOfCity.DataTypeOutput;

namespace WeatherOfCity.Sevices
{
    public interface IWeatherCity
    {
        public List<WeatherCityDTO> get(int page);
        public List<dynamic> getweather(DateTime begin, DateTime end);
        public WeatherCityDTO get_max();
        public WeatherCityDTO get_min();
    }
}
