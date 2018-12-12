using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Utilities.Logs
{
    /**
     * Класс для логирования.
     * В приложении необходимо установить пакеты: NLog и NLog.Config
     */
    public class LogTools
    {
        protected String logName = "debuglogs";
        protected String logDefaultName = "Default";

        protected String df_dateonly = "dd.MM.yyyy hh:mm:ss";
        protected String df_dateonly_full = "dd.MM.yyyy HH:mm:ss,SSS";

        protected Logger log;


        /// <summary>
        /// Получение класса для логирования
        /// </summary>
        /// <returns></returns>
        public static LogTools GetDebugLog()
        {
            LogTools lt = new LogTools();
            return lt;
        }

        /// <summary>
        /// Получение класса для логирования с 
        /// </summary>
        /// <param name="logNameIn"></param>
        /// <returns></returns>
        public static LogTools GetErrorLog()
        {
            LogTools lt = new LogTools("exceptionlog");
            return lt;
        }


        /// <summary>
        /// Получение базового лога
        /// </summary>
        /// <returns></returns>
        protected Logger GetLog()
        {
            if (log == null)
            {
                log = LogManager.GetCurrentClassLogger();
            }
            return log;
        }



        /// <summary>
        ///  Получение лога с заданными именем
        /// </summary>
        /// <param name="logNameIn"></param>
        /// <returns></returns>
        protected Logger GetLog(String logNameIn)
        {
            log = LogManager.GetLogger(logNameIn);
            return log;
        }

        /// <summary>
        /// Создание LogTools debuglogs лога
        /// </summary>
        public LogTools()
        {
            logName = "debuglogs";
        }


        /// <summary>
        /// Создание лога с нужным типом
        /// </summary>        
        public LogTools(String logNameIn)
        {
            GetLog(logNameIn);
            logName = logNameIn;
        }

        /// <summary>
        /// Получить текущую дату время в формате dd.MM.yyyy HH:mm:ss
        /// </summary>
        /// <returns></returns>
        protected String getDate()
        {
            DateTime currentDate = DateTime.Now;

            return currentDate.ToString(df_dateonly_full);
        }

        /// <summary>
        /// Запись информации в лог + дата + текущий пользователь
        /// </summary>
        /// <param name="str"></param>
        protected void WriteLn(Object str)
        {
            StringBuilder sb = GetStr(str);

            GetLog().Info(sb.ToString());
        }

        /// <summary>
        /// Запись информации в лог + дата + текущий пользователь
        /// </summary>
        /// <param name="str"></param>
        public void WriteDebug(Object str)
        {
            StringBuilder sb = GetStr(str);

            GetLog().Info(sb.ToString());
        }

        /// <summary>
        /// Получить строку
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private StringBuilder GetStr(object str)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(getDate());
            //sb.Append(" ");
            sb.Append(str);
            return sb;
        }

        /// <summary>
        /// Запись в лог сообщения с Exception
        /// </summary>
        /// <param name="str"></param>
        /// <param name="err"></param>
        public void WriteExceprion(Object str, Exception err)
        {
            StringBuilder sb = GetStr(str);
            GetLog().Error(err, sb.ToString());
        }

    }

}
