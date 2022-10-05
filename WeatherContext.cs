using Microsoft.EntityFrameworkCore;
using WeatherOfCity.Models;

namespace WeatherOfCity
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions opt) : base(opt)
        { }
        #region DBSet
        public DbSet<City> Citys { get; set; }
        public DbSet<Times> Times { get; set; }
        public DbSet<Weather> Weather { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<City>(e =>
            {
                e.ToTable(nameof(City))
                .HasComment("Bảng Cty")
                .HasKey(ct => ct.City_Id);

                e.Property(ct => ct.City_Id)
                .UseIdentityColumn(1, 1);
            });
            model.Entity<Times>(e =>
            {
                e.ToTable(nameof(Times));
                e.HasKey(e => e.time_Id);

                e.HasOne(e => e.City)
                .WithMany(t => t.times)
                .HasForeignKey("City_Id");
            });
            model.Entity<City>(e =>
            {
                e.HasIndex(e => e.City_Id).IsUnique();
                e.Property(e => e.City_Name).IsRequired().HasMaxLength(20);
            });
        }
    }
}
