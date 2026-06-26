using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobAppFlow.SqlDataAccess.Data;

public sealed class JobAppFlowDbContext
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
    public JobAppFlowDbContext(DbContextOptions<JobAppFlowDbContext> options)
        : base(options)
    {
    }
}
