using Dapper;
using System.Data;
using System.Data.SqlClient;
using WeatherOfCity.DataTypeOutput;

namespace WeatherOfCity.Sevices
{
    public class WeatherCity : IWeatherCity
    {
        private readonly WeatherContext _context;
        private readonly IConfiguration _confi;
        public int page_size { get; set; } = 7;

        public WeatherCity(WeatherContext context, IConfiguration confi)
        {
            _context = context;
            _confi = confi;
        }

        public List<WeatherCityDTO> get(int page = 1)
        {
           var result = new List<WeatherCityDTO>();
           var value = new List<WeatherCityDTO>();
            using (var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                var temp = connect.Query<WeatherCityDTO>(
                    "dbo.SP_SELECT_WeatherCity",
                    commandType: CommandType.StoredProcedure
                    );
                temp = temp.Skip((page - 1) * page_size).Take(page_size);
                result = temp.ToList();
            }

            for(int i=0;i< result.ToArray().Length-1; i++)
            {
                if(result[i].TemperatureC- result[i+1].TemperatureC>=4 )
                {
                    value.Add(result[i]);
                }
            }
            return value;
        }

        public List<dynamic> getweather(DateTime begin, DateTime end)
        {
            var result = new List<dynamic>();
            using(var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                var hehe = new DynamicParameters();
                hehe.Add("@DayBegin",begin);
                hehe.Add("@DayEnd", end);
                var temp = connect.Query<dynamic>(
                    "dbo.SP_PIVOT_CITY",
                    hehe,
                    commandType: CommandType.StoredProcedure
                    );
                result = temp.ToList();
            }
            return result;
        }

        public WeatherCityDTO get_max()
        {
            var result = new WeatherCityDTO();
            using (var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                var temp = connect.Query<WeatherCityDTO>(
                    "dbo.[SP_CITY_TEMP_MAX]",
                    commandType : CommandType.StoredProcedure
                    );
                result = temp.FirstOrDefault();
            }
            return result;
            }

        public WeatherCityDTO get_min()
        {
            var result = new WeatherCityDTO();
            using (var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                var temp = connect.Query<WeatherCityDTO>(
                    "dbo.[SP_CITY_TEMP_Min]",
                    commandType: CommandType.StoredProcedure
                    );
                result = temp.FirstOrDefault();
            }
            return result;
        }
    }
}
