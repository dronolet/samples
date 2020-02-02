using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace apitest.Objects
{
    public class Account
    {
        [Key]
        public int id { get; set; }

        [Column(TypeName = "varchar(20)")]
        [Required]
        public string account_number { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [ConcurrencyCheck]                                // токен параллелизма
        public decimal balance { get; set; }

        public List<AccountHistory> AccountAccountHistorys { get; set; }

        public Account()
        {
            AccountAccountHistorys = new List<AccountHistory>();
        }

    }
}
