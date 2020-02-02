using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
   
    [Table("SprBudjetUserONOR")]
    public class SprBudjetUser
    {
        [Key]
        [Column("SprBudjetUser_ID")]
        public int Id { get; set; }

        [Column("BudjetUserName ")]
        public string FullName { get; set; }

        public string UserLogin { get; set; }

        public string Password { get; set; }

        [Column("SprBudjetUpravlenie_ID")]
        public int? UpravlenieID { get; set; }

        [Column("Rukovoditel_ID")]
        public int? Rukovoditel { get; set; }

        public string email { get; set; }
    }
}
