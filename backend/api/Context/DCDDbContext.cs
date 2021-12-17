using Microsoft.EntityFrameworkCore;
using api.Models;


namespace api.Context
{
    public class DCDDbContext : DbContext
    {
        public DCDDbContext(DbContextOptions<DCDDbContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }

    }
}
