using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherOfCity.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        [ForeignKey(nameof(user_id))]
        public Users Users { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
