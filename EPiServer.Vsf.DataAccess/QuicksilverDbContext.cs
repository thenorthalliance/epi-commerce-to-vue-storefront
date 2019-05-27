using System.Data.Entity;
using EPiServer.Vsf.DataAccess.Model;

namespace EPiServer.Vsf.DataAccess
{
    public class QuicksilverDbContext : DbContext
    {
        public QuicksilverDbContext() : base("EcfSqlConnection")
        {
            
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .ToTable("AspNetUsers")
                .HasOptional(s => s.RefreshToken)
                .WithRequired(s => s.User);

            modelBuilder.Entity<RefreshToken>()
                .ToTable("AspNetRefreshTokens");
        }
    }
}
