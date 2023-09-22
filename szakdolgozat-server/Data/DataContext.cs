using Microsoft.EntityFrameworkCore;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Data
{
    public class DataContext: DbContext
    {
        public virtual DbSet<Flower> Flowers { get; set; }
        public virtual DbSet<CroppedImage> CroppedImages { get; set; }
        public virtual DbSet<CaptureFrequency> CaptureFrequencies { get; set; }

        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaptureFrequency>().ToTable("CaptureFrequency");
            modelBuilder.Entity<Flower>().ToTable("Flower");
            modelBuilder.Entity<CroppedImage>().ToTable("CroppedImage");
        }
    }
}
