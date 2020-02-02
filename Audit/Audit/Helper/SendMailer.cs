using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Helper
{
    public class SendMailer
    {
        public static string SendMail(
                                        string from,
                                        string to,
                                        string subject,
                                        string mailbody,
                                        List<string> attaches = null
                                     )
        {
            string res = "";
            try
            {
                var msg = new MailMessage(from, to, subject, mailbody);

                if (attaches != null)
                {
                    foreach (string itemAttach in attaches)
                    {
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(itemAttach);
                        msg.Attachments.Add(attachment);
                    }
                }

                msg.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient("10.10.0.27", 25); //172.16.1.2
                smtpClient.EnableSsl = false;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}
