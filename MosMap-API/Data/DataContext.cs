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
        public DbSet<User> Users { get; set; }
    }
}