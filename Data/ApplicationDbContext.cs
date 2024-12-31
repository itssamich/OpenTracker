using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenTracker.Data.Models;

namespace OpenTracker.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    //public DbSet<Card> Cards { get; set; }
    //public DbSet<Session> Sessions {get; set; }
}
