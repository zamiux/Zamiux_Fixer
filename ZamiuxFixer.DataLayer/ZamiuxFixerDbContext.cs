using Microsoft.EntityFrameworkCore;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer
{
    // Agent DataBase
    public class ZamiuxFixerDbContext:DbContext
    {
        #region Constructor

        #endregion


        #region DbSet Entites
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        #region Database Connection 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection Service, i want use (sql file local) in VS 
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZamiuxFixer_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            base.OnConfiguring(optionsBuilder);
        }
        #endregion
    }
}
