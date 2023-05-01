using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SalonWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ClientAdministrate> ClientManagment { get; set; }
    }
}
