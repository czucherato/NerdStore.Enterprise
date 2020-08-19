using Jwks.Manager;
using Microsoft.EntityFrameworkCore;
using Jwks.Manager.Store.EntityFrameworkCore;
using NerdStore.Enterprise.Identidade.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NerdStore.Enterprise.Identidade.API.Data
{
    public class ApplicationDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base (options) {}

        public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}