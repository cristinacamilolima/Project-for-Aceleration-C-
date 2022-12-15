using Microsoft.EntityFrameworkCore;
using Project_for_Aceleration_Csharp_Tryitter.Models;

namespace Project_for_Aceleration_Csharp_Tryitter.Repository
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Course> Courses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=master;User=SA;Password=password123!;");
        }
    }
}
