using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HammerProject.Server
{
    public class HammerProjectContext : IdentityDbContext<User>
    {
        public HammerProjectContext(
            DbContextOptions options) : base(options)
        {
        }

        public DbSet<department> department { get; set; }
        public DbSet<employee> employee { get; set; }
        public DbSet<login> login { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
