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
    public class LogTools : ILogTools
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
        public static ILogTools GetLog()
        {
            ILogTools lt = new LogTools();
            return lt;
        }


        /// <summary>
        /// Получение базового лога
        /// </summary>
        /// <returns></returns>
        protected Logger GetCurrenLog()
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
        protected Logger GetCurrenLog(String logNameIn)
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
            GetCurrenLog(logNameIn);
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
        //protected void WriteLn(Object str)
        //{
        //    StringBuilder sb = GetStr(str);

        //    GetCurrenLog().Info(sb.ToString());
        //}

        /// <summary>
        /// Запись информации в лог + дата + текущий пользователь
        /// </summary>
        /// <param name="str"></param>
        public void WriteLn(Object str)
        {
            StringBuilder sb = GetStr(str);
            GetCurrenLog().Info(sb.ToString());

            Console.WriteLine(".." + str);
        }


        /// <summary>
        /// Запись в лог сообщения с Exception
        /// </summary>
        /// <param name="str"></param>
        /// <param name="err"></param>
        public void WriteLn(Object str, Exception err)
        {
            StringBuilder sb = GetStr(str);
            GetCurrenLog().Error(err, sb.ToString());

            Console.WriteLine(".." + str + err.Message);
        }

        /// <summary>
        /// Получить строку
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected StringBuilder GetStr(object str)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(getDate());
            //sb.Append(" ");
            sb.Append(str);
            return sb;
        }
    }
}
