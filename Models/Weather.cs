using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherOfCity.Models
{
    [Table("Weather")]
    public class Weather
    {
        [ForeignKey("Times")]
        [Key]
        public int Weather_Id { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public int time_Id { get; set; }
        public virtual Times Times { get; set; }
    }

}
