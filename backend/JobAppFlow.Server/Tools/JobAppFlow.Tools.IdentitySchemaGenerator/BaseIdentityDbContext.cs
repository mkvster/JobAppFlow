using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobAppFlow.Tools.IdentitySchemaGenerator;

public sealed class BaseIdentityDbContext<TKey>
    : IdentityDbContext<
        IdentityUser<TKey>,
        IdentityRole<TKey>,
        TKey,
        IdentityUserClaim<TKey>,
        IdentityUserRole<TKey>,
        IdentityUserLogin<TKey>,
        IdentityRoleClaim<TKey>,
        IdentityUserToken<TKey>> where TKey : IEquatable<TKey>
{
    public BaseIdentityDbContext(DbContextOptions<BaseIdentityDbContext<TKey>> options)
        : base(options)
    {
    }
}
