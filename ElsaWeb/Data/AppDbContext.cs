using ElsaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ElsaWeb.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
