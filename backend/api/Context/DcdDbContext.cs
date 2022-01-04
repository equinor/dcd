using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Context
{
    public class DcdDbContext : DbContext
    {
        public DcdDbContext(DbContextOptions<DcdDbContext> options) : base(options)
        {

        }
        public DbSet<Project>? Projects { get; set; }

    }
}
