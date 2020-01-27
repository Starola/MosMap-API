using Microsoft.EntityFrameworkCore;
using MosMap_API.Models;

namespace MosMap_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
       /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=library;user=localhost;password=");
        }*/
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<SubCategoryLocation> SubCategoryLocations { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}