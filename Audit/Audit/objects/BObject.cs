using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    public class BObject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
