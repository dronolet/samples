using System;
using System.ComponentModel.DataAnnotations;

namespace apitest.Models
{
    public class EmployeesListModel 
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public string Number { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public int CompanyId { get; set; }
    }
}
