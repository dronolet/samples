using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.models
{
    public class EmailModel
    {
        [Required]
        public string Email { get; set; }
    }
}
