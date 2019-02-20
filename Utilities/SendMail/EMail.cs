using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Utilities.SendMail 
{
    public class EMail
    {
        public const string EMailLogin = "qcoderitest@gmail.com";
        public const string EMailPassword = "NGY69Zrme4MFAT4";
        public const string EMailHost = "smtp.gmail.com";
        public const int EMailPort = 465; //587;


        /// <summary>
        /// Метод для отправки
        /// </summary>
        /// <param name="msg">текст сообщения</param>
        /// <param name="subject">заголовок</param>
        /// <param name="emails">список емейлов для отправки</param>
        public static async Task PostMail(String msg, String subject, String[] emails)
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

                    emailMessage.From.Add(new MailboxAddress("Администрация сайта", EMailLogin));

                    foreach (var email in emails)
                        emailMessage.To.Add(new MailboxAddress("", email));

                    emailMessage.Subject = subject;

                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = msg
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(EMailHost, EMailPort, true);
                        await client.AuthenticateAsync(EMailLogin, EMailPassword);
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
