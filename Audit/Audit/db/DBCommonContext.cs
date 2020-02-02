using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using Audit.objects;

namespace audit.db
{
    public class DBCommonContext: DbContext
    {


        [NotMapped]
        public DbSet<SprBudjetUser> SprBudjetUsers { get; set; }

        [NotMapped]
        public DbSet<Order> Orders { get; set; }

        [NotMapped]
        public DbSet<OrderFile> OrderFiles { get; set; }

        [NotMapped]
        public DbSet<BObject> BObjects { get; set; }

        [NotMapped]
        public DbSet<Kontragent> Kontragents { get; set; }

        [NotMapped]
        public DbSet<HeadStroyUser> HeadStroyUsers { get; set; }

        [NotMapped]
        public DbSet<MainEnginier> MainEnginiers { get; set; }

        [NotMapped]
        public DbSet<HeadManager> HeadManagers { get; set; }

        [NotMapped]
        public DbSet<ReportSatistic> ReportSatistics { get; set; }
        


        public DBCommonContext(DbContextOptions<DBCommonContext> options)
            : base(options)
        {
            
        }

       

    }
}
