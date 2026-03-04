using Microsoft.EntityFrameworkCore;
using Search.API.Data.Models;

namespace Search.API.Data.Contexts
{
    public class TodoContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Todo> Todos => Set<Todo>();
    }
}
