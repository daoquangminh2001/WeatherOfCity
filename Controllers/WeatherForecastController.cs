using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherOfCity.Sevices;
namespace WeatherOfCity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherCity _index;
        public WeatherForecastController( IWeatherCity index)
        {
            _index = index;
        }
        [HttpGet,Authorize(Roles = "admin")]

        public async Task<IActionResult> Getall(int page =1)
        {
            try
            {
                var result = _index.get(page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("hehe")]
        public async Task<IActionResult> Getweather(DateTime begin, DateTime end)
        {
            try
            {
                var value = _index.getweather(begin, end);
                return Ok(value);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("getmaxvalue")]
        public async Task<IActionResult> Getmax()
        {
            try
            {
                var value = _index.get_max();
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }   
        [HttpGet, Route("getminvalue")]
        public async Task<IActionResult> Getmin()
        {
            try
            {
                var value =  _index.get_min();
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 
}