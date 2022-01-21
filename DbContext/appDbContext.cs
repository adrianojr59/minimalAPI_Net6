using Microsoft.EntityFrameworkCore;

namespace minimalAPI_Net6
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> option) : base(option) => Database.EnsureCreated();

        public DbSet<Person>PersonList { get; set; }
    
    }
}
