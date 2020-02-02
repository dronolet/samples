using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apitest.Objects
{
    public class AccountHistory
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }


        [Required]
        [Column(TypeName = "DateTime")]    
        public DateTime ChangedAt { get; set; }

    }
}
