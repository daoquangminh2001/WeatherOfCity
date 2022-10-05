using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherOfCity.Models
{
    [Table("Times")]
    public class Times
    {
        [Key]
        public int time_Id { get; set; }
        public DateTime? Time { get; set; }
        public virtual City City { get; set; }
        public virtual Weather Weather { get; set; }
        public Times()
        {
            Weather = new Weather();
        }
    }
}
