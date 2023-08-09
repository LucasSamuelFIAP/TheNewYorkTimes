using Microsoft.EntityFrameworkCore;
using TheNewYorkTimes.Models;

namespace TheNewYorkTimes.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Noticia> Noticia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
