using IdentityCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence;

public class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}