using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherOfCity.Models
{
        [Table("City")]
        public class City
        {
            [Key]
            public int City_Id { get; set; }
            [Required]
            public string City_Name { get; set; }

            public ICollection<Times> times { get; set; }

            public City()
            {
                times = new List<Times>();
            }
        }
    }
