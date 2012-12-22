using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;

namespace arunes
{
    public class EMailMessage
    {
        public string SmtpServer;
        public int SmtpPort;
        public bool SmtpUseSSL;
        public string SmtpUser;
        public string SmtpPassword;
        
        public string FromMail;
        public string FromName;
        public string ToMail;
        public string ToName;
        public string Subject = "no-subject";
        public string Message;
        public Exception LastException;

        public EMailMessage()
        { 
            
        }

        public EMailMessage(string smtpServer, int smtpPort, bool smtpUseSSL, string smtpUser, string smtpPassword)
        {
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            SmtpUseSSL = smtpUseSSL;
            SmtpUser = smtpUser;
            SmtpPassword = smtpPassword;
        }

        public bool SendMail()
        {
            MailAddress fromMail = new MailAddress(FromMail, FromName, System.Text.Encoding.UTF8);
            MailAddress toMail = new MailAddress(ToMail, ToName, System.Text.Encoding.UTF8);

            var smtp = new SmtpClient
            {
                Host = SmtpServer,
                Port = SmtpPort,
                EnableSsl = SmtpUseSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPassword)
            };

            try
            {
                using (var message = new MailMessage(fromMail, toMail)
                {
                    Sender = toMail, // hesap kiminse o göndersin
                    ReplyTo = fromMail,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true,
                    Subject = Subject,
                    Body = Message
                })
                {
                    smtp.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                return false;
            }
        }
    }
}