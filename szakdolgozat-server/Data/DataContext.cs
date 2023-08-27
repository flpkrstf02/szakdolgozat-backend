using Microsoft.EntityFrameworkCore;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Data
{
    public class DataContext: DbContext
    {
        public virtual DbSet<Flower> Flowers { get; set; }
        public virtual DbSet<CroppedImage> CroppedImages { get; set; }
        protected readonly IConfiguration Configuration;

        //public DataContext(DbContextOptions<DataContext> option):base(option)
        //{

        //}

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flower>().ToTable("Flower");
            modelBuilder.Entity<CroppedImage>().ToTable("CroppedImage");
        }
    }
}
