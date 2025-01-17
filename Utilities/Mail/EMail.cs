﻿using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Utilities.Logs;

namespace Utilities.Mail
{
    /// <summary>
    /// Отправка сообщения
    /// </summary>
    public class EMail : IMail
    {
        private LogTools lgError = new ExceptionLog();
        private LogTools lg = new LogTools();
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
                    Settings st = new Settings();
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Администрация сайта", st.GetString("Email_Login")));

                    foreach (var email in emails)
                        emailMessage.To.Add(new MailboxAddress("", email));

                    emailMessage.Subject = subject;
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = msg
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(st.GetString("Email_Host"), Int32.Parse(st.GetString("Email_Port")), true);
                        await client.AuthenticateAsync(st.GetString("Email_Login"), st.GetString("Email_Password"));
                        await client.SendAsync(emailMessage);
                        await client.DisconnectAsync(true);
                    }
                    lg.WriteLn(" отрпавкf сообщения:" + msg);
                }
                catch (Exception ex)
                {
                    lgError.WriteLn("Ошибка отрпавки сообщения:" + msg, ex);
                    throw ex;
                }
                finally
                {
                }
            }
        }
    }
}
