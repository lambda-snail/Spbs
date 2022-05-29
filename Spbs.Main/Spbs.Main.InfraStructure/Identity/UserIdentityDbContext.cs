using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Spbs.Main.InfraStructure.Identity;

public class UserIdentityDbContext : IdentityDbContext
{
    public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options)
        : base(options)
    {
    }
    
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    // }
}