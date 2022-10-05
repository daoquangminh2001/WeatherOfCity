using WeatherOfCity.DataTypeOutput;
using WeatherOfCity.Input;

namespace WeatherOfCity.Sevices
{
    public interface ICityRespornsitory
    {
        public List<CityDTO> GetCity();
        public void Insert_UpdateCity(List<CityInput> input);

        public void DeleteCity(int id);
        public void Delete_City(List<int> index);
    }
}