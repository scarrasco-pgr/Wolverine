using Microsoft.EntityFrameworkCore;
using Search.API.Data.Models;

namespace Search.API.Data.Contexts
{
    public class TodoContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_trgm");
            modelBuilder.HasPostgresExtension("fuzzystrmatch");
            modelBuilder.Entity<Todo>()
                .HasIndex(t => t.Description)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
        }
    }
}
