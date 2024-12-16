using GymAppReal.Models;
using Microsoft.EntityFrameworkCore;

namespace GymAppReal.Data
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Exercises)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
