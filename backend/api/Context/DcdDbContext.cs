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

        public DbSet<Case>? Cases { get; set; }

        public DbSet<DrainageStrategy>? DrainageStrategies { get; set; }

        public DbSet<ProductionProfileOil>? ProductionProfileOil { get; set; }

        public DbSet<ProductionProfileGas>? ProductionProfileGas { get; set; }

    }
}
