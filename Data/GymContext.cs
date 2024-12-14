using GymAppReal.Models;
using Microsoft.EntityFrameworkCore;

namespace GymAppReal.Data
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options) : base(options)
        {
        }
        public DbSet<Exercise> Exercises { get; set; }
    }
}
