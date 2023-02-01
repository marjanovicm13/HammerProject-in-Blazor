using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HammerProject.Server
{
    public class HammerProjectContext : ApiAuthorizationDbContext<Login>
    {
        public HammerProjectContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<department> department { get; set; }
        public DbSet<employee> employee { get; set; }
    }
}
