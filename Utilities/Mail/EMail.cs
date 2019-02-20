using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Utilities.Mail 
{
    public class EMail : IMail
    {
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="msg">текст сообщения</param>
        /// <param name="subject">тема сообщения</param>
        /// <param name="emails">список получателей</param>
        /// <returns></returns>
        public /*static*/ async Task PostMail(String msg, String subject, String[] emails)
        {
            if (emails != null && emails.Length > 0)
            {
                try
                {
                    //// создаем объект сообщения
                    //using (var mail = new MailMessage())
                    //{
                    //    //MailMessage mail = new MailMessage();
                    //    // наш email с заголовком письма 
                    //    mail.From = new MailAddress(EMailLogin); //, email.Head, );
                    //    // кому отправляем
                    //    foreach (var email in emails)
                    //    {
                    //        mail.To.Add(new MailAddress(email));
                    //    }

                    //    // тема письма
                    //    mail.Subject = subject;
                    //    // текст письма - включаем в него ссылку
                    //    mail.Body = msg;
                    //    mail.IsBodyHtml = true;

                    //    // адрес smtp-сервера, с которого мы и будем отправлять письмо
                    //    using (var smtpClient = new SmtpClient(EMailHost, EMailPort))
                    //    {
                    //        smtpClient.Credentials = new System.Net.NetworkCredential(EMailLogin, EMailPassword);
                    //        smtpClient.EnableSsl = true;
                    //        await smtpClient.SendMailAsync(mail);
                    //    }
                    //}

                    var emailMessage = new MimeMessage();

                    emailMessage.From.Add(new MailboxAddress("Администрация сайта", Settings.EMailLogin));

                    foreach (var email in emails)
                        emailMessage.To.Add(new MailboxAddress("", email));

                    emailMessage.Subject = subject;

                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = msg
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(Settings.EMailHost, Settings.EMailPort, true);
                        await client.AuthenticateAsync(Settings.EMailLogin, Settings.EMailPassword);
                        await client.SendAsync(emailMessage);

                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
        }
    }
}
