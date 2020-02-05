using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace apitest.Objects
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Fio { get; set; }
        public string Number { get; set; }

        [Required]
        [Column(TypeName = "DateTime")]
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public int  CompanyId { get; set; }
    }
}
