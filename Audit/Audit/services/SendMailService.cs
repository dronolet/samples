using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Audit.interfaces;
using Audit.models;

namespace Audit.services
{
    public class SendMailService : ISendMailService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SendMailService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }


        public string SendMail(
                                        string from,
                                        string to,
                                        string subject,
                                        string mailbody,
                                        List<FileAttachment> attaches = null
                                     )
        {
            string res = "";
            try
            {
                using (MailMessage msg = new MailMessage(from, to))
                {
                    msg.Subject = subject;
                    msg.Body = mailbody;

                    if (attaches != null)
                    {
                        foreach (FileAttachment itemAttach in attaches)
                        {
                            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(itemAttach.FilePath);
                            attachment.ContentDisposition.FileName = itemAttach.FileName;
                            msg.Attachments.Add(attachment);
                        }
                    }

                    msg.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient("10.10.0.27", 25);
                    smtpClient.EnableSsl = false;
                    smtpClient.Send(msg);
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            finally {
                GC.Collect();
            }
            return res;
        }

    }
}
