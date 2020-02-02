using System;
using System.ComponentModel.DataAnnotations;

namespace apitest.Models
{
    public class AcountHistoryModel
    {
        public int Id { get; set; }
        public string Account_number { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
