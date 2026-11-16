using Microsoft.EntityFrameworkCore;

namespace TodoApiDotNet.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}