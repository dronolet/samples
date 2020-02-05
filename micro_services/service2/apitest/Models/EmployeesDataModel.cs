using System;
using System.ComponentModel.DataAnnotations;

namespace apitest.Models
{
    public class EmployeesDataModel
    {
        public string Fio { get; set; }
        public string Number { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
    }
}
