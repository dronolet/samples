using System.Collections.Generic;
using Audit.models;

namespace Audit.interfaces
{
    public interface ISendMailService
    {
        string SendMail(
                                        string from,
                                        string to,
                                        string subject,
                                        string mailbody,
                                        List<FileAttachment> attaches = null
                                     );
    }
}
