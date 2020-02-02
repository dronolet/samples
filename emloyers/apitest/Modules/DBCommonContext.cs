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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHistory> AccountHistorys { get; set; }



        public DBCommonContext(DbContextOptions<DBCommonContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountHistory>()
             .Property(b => b.ChangedAt)
             .HasDefaultValueSql("getdate()");

            
            modelBuilder.Entity<Account>()
              .HasData(
                new Account { id = 1, account_number = "40817810099910004312", balance = 0 },
                new Account { id = 2, account_number = "40702810038210100536", balance = 0 },
                new Account { id = 3, account_number = "30101810400000000225", balance = 0 },
                new Account { id = 4, account_number = "40702810838120108695", balance = 0 }
              );
        }


    }
}
