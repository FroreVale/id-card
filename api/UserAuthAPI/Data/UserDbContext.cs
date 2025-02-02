using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Models; 

namespace UserAuthAPI.Data 
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
