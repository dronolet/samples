using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.models
{
    public class UserInfoModel
    {

        public string FullName { get; set; }

        public int isHead { get; set; }

        public int isRevisor { get; set; }
    }
}
