using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.models
{
    public class FileListModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }
    }
}
