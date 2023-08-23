using Microsoft.EntityFrameworkCore;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Data
{
    public class DataContext: DbContext
    {
        public virtual DbSet<Flower> Flowers { get; set; }
        public virtual DbSet<CroppedImage> CroppedImages { get; set; }

        public DataContext(DbContextOptions<DataContext> option):base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flower>().ToTable("Flower");
            modelBuilder.Entity<CroppedImage>().ToTable("CroppedImage");
        }
    }
}
