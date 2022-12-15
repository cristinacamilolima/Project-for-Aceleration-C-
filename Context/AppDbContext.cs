using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using Project_for_Aceleration_Csharp_Tryitter.Utils;
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

        public User? GetUser(StringValues token)
        {
            var claims = Token.GetTokenClaims(token);

            var email = claims.Claims.ElementAt(0).Value;

            return Users!.AsNoTracking().FirstOrDefault(user => user.Email == email);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TryitterDB;User=SA;Password=password123!;trustservercertificate=true");
        }
    }
}
