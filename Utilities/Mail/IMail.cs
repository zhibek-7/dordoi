using System;
using System.Threading.Tasks;

namespace Utilities.Mail
{
    public interface IMail
    {
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="msg">текст сообщения</param>
        /// <param name="subject">тема сообщения</param>
        /// <param name="emails">список получателей</param>
        /// <returns></returns>
        Task PostMail(String msg, String subject, String[] emails);
    }
}
