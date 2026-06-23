using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobAppFlow.Tools.IdentitySchemaGenerator.Data;

public sealed class JobAppFlowIdentityDbContext
    : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
{
    public JobAppFlowIdentityDbContext(DbContextOptions<JobAppFlowIdentityDbContext> options)
        : base(options)
    {
    }
}
