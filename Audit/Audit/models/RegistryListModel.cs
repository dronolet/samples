using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.models
{
    public class RegistryListModel: RegistryModel
    {

        public string BuildObjectName { get; set; }

        public string ResultCaption { get; set; }

        public int? HeadId { get; set; }

        public int isOverTime { get; set; }


        public string TimeInterval {
            get {
                return DBegin.ToString("HH:mm") + " - " + DEnd.ToString("HH:mm");
            }
        }
    }
}
