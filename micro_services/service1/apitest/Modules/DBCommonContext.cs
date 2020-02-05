using Microsoft.EntityFrameworkCore;
using apitest.Objects;
using apitest.Objects;


namespace apitest.Modules
{
    /// <summary>
    /// Контекст взаимодействия с БД
    /// </summary>
    public class DBCommonContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DBCommonContext(DbContextOptions<DBCommonContext> options)
           : base(options)
        {
           Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}
