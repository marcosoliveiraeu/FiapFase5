
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class DbContextIdentity : Microsoft.EntityFrameworkCore.DbContext
    {

        public DbContextIdentity(DbContextOptions<DbContextIdentity> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }



    }
}
