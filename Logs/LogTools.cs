using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Models.Logs
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

        /**
         * Получение базового лога
         */
        protected Logger getLog()
        {
            if (log == null)
            {
                log = LogManager.GetCurrentClassLogger();
            }
            return log;
        }


        /**
         * Получение  лога с заданными именем
         */
        protected Logger getLog(String logNameIn)
        {
            return LogManager.GetLogger(logNameIn);
        }

        // Создание debuglogs лога
        public LogTools()
        {
            logName = "debuglogs";
        }


        // Создание лога с базовым контекстом
        public LogTools(String logNameIn)
        {
            getLog();
            logName = logNameIn;
        }

        /**
        * Текущая дата время в формате dd.MM.yyyy HH:mm:ss
        * 
        * @return
         */
        protected String getDate()
        {
            DateTime currentDate = DateTime.Now;

            return currentDate.ToString(df_dateonly_full);
        }


        /**
        * Запись информации в лог + дата + текущий пользователь
        * 
        * @param str
        */
        protected void WriteLn(Object str)
        {
            StringBuilder sb = getStr(str);

            getLog().Info(sb.ToString());
        }

        private StringBuilder getStr(object str)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(getDate());
            //sb.Append(" ");
            sb.Append(str);
            return sb;
        }

        public void WriteDebug(Object str)
        {
            StringBuilder sb = getStr(str);

            getLog().Info(sb.ToString());
        }

        public void WriteExceprion(Object str, Exception err)
        {
            StringBuilder sb = getStr(str);
            getLog().Error(err, sb.ToString());

            /*
                        logger.Trace("trace message");
                        logger.Debug("debug message");
                        logger.Info("info message");
                        logger.Warn("warn message");
                        logger.Error("error message");
                        logger.Fatal("fatal message");
                        */
        }

    }

}
