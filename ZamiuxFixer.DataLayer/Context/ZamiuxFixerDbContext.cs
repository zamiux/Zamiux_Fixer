using Microsoft.EntityFrameworkCore;
using ZamiuxFixer.Domain.Questions;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Context
{
    // Agent DataBase
    public class ZamiuxFixerDbContext : DbContext
    {
        #region Constructor
        public ZamiuxFixerDbContext(DbContextOptions<ZamiuxFixerDbContext> options):base(options)
        {
             
        }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        #endregion


        #region DbSet Entites
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        #endregion

        #region Question
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<QuestionFavorite> QuestionFavorites { get; set; }
        public DbSet<QuestionVote> QuestionVotes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        #endregion

        #region Database Connection 
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Connection Service, i want use (sql file local) in VS 
        //    optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZamiuxFixer_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //    base.OnConfiguring(optionsBuilder);
        //}
        #endregion
    }
}
