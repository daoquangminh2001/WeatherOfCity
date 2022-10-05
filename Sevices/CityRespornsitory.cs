using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using WeatherOfCity.DataTypeOutput;
using WeatherOfCity.Input;

namespace WeatherOfCity.Sevices
{
    public class CityRespornsitory: ICityRespornsitory
    {
        private readonly WeatherContext _context;
        private readonly IConfiguration _confi;
        public CityRespornsitory(WeatherContext context, IConfiguration confi)
        {
            _context = context;
            _confi = confi;
        }

        public void DeleteCity(int id)
        {
            using(var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                if (connect.State == ConnectionState.Closed) connect.Open();
                var result = connect.Query<CityDTO>(
                    "DBO.SP_CITY_DELETE",
                    this.SetParameters1(id),
                    commandType: CommandType.StoredProcedure
                    );
            }
        }

        public void Delete_City(List<int> index)
        {
            using (var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                XElement xml = null;
                xml = new XElement("items",
                    from x in index
                    select new XElement("item",x)
                    );
                var result = new DynamicParameters();
                result.Add("@xml", xml);
                var temp = connect.Query<CityDTO>(
                    "DBO.SP_CITY_DELETE",
                    result,
                    commandType: CommandType.StoredProcedure
                    );
            }
        }
        #region Get city
        public List<CityDTO> GetCity()
        {
            var result = new List<CityDTO>();
            using(var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                var temp = connect.Query<CityDTO>(
                    "DBO.SP_CITY_SELECT",
                    commandType: CommandType.StoredProcedure
                    );
                result = temp.ToList();
            }
            return result;
        }
        #endregion
        #region insert update
        public void Insert_UpdateCity(List<CityInput> input)
        {
            using (var connect = new SqlConnection(_confi.GetSection("ConnectionStrings:constring").Value))
            {
                XElement xml = null;
                xml = new XElement("items",
                       from detail in input
                       select new XElement("item",
                      new XAttribute("City_Id", detail.City_Id),
                       new XElement("City_Id", detail.City_Id),
                       new XElement("City_Name", detail.City_Name)
                       ));
                var result = new DynamicParameters();
                result.Add("@xml", xml);
                var temp = connect.Query<CityInput>(
                    "DBO.SP_CITY_INSERT_Muntipler",
                    result,
                    commandType: CommandType.StoredProcedure
                    );
            }

        }
        #endregion
        private DynamicParameters SetParameters(CityInput input)
        {
            var result = new DynamicParameters();
            result.Add("@City_Id", input.City_Id);
            result.Add("@City_Name", input.City_Name);
            return result;
        }
        private DynamicParameters SetParameters1(int id)
        {
            var result = new DynamicParameters();
            result.Add("@Id", id);
            return result;
        }
    }
}
