using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{

    [Table("Kontragents")]
    public class Kontragent
    {
        [Key]
        [Column("1S_Kontragent_ID")]  
        public int Id { get; set; }
        
        [Column("KontragentName")]
        public string name { get; set; }

        public string email { get; set; }
    }
}
