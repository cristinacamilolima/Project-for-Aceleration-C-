using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using System;

namespace Project_for_Aceleration_Csharp_Tryitter.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post>? Posts { get; set; }
        public DbSet<User>? Users { get; set; }

        public User? GetUser(Guid id)
        {
            return Users!.Include(p => p.Posts).FirstOrDefault(u => u.UserId == id);
        }
        public User? GetUser(string email)
        {
            return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TryitterDB;User=SA;Password=password123!;trustservercertificate=true");
        }
    }
}
