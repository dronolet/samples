using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.models
{
    public class OnorUserModel
    {
        public int Id  { get; set; }

        public string FullName { get; set; }

        public string Name {
            get {
                string[] parts = FullName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                return (parts.Length > 0 ? parts[0] + (parts.Length > 1? parts[1] + ". " + (parts.Length > 2? parts[2] = ". ":"") : "" )  : "");
            }
        }

    }
}
