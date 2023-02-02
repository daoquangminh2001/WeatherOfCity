using Microsoft.AspNetCore.Mvc;
using WeatherOfCity.Attributes;
using WeatherOfCity.Input;
using WeatherOfCity.Sevices;
namespace WeatherOfCity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityControllers : ControllerBase
    {
        private readonly WeatherContext _context;
        private readonly ICityRespornsitory _city;
        public CityControllers(WeatherContext context, ICityRespornsitory city)
        {
            _context = context;
            _city = city;
        }
        [HttpGet]
        [Cache(100)]
        public async Task<IActionResult> GetAllCity()
        {
            try
            {
                var result = _city.GetCity();
                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("insertupdatecity")]
        public async Task<IActionResult> insert(List<CityInput>? input)
        {
            try
            {
                _city.Insert_UpdateCity(input);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("deletecity")]
        public async Task<IActionResult> delete(List<int> index)
        {
            try
            {
                _city.Delete_City(index);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
