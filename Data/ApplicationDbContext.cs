using DatingApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) { }

        public DbSet<AppUser>appUsers { get; set; }
        
    }
}
