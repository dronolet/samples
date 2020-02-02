using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.objects;

namespace audit.db
{
    public class DBDictinaryContext: DbContext
    {
        [NotMapped]
        public DbSet<Holiday> Holidays { get; set; }


        public DBDictinaryContext(DbContextOptions<DBDictinaryContext> options)
            : base(options)
        {
            
        }

       

    }
}
