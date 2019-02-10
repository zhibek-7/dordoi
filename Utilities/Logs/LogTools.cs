using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
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


            Console.WriteLine(".." + sb.ToString());
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

            Console.WriteLine(".." + str + " " + err?.Message);
        }

        /// <summary>
        /// Запись в лог сообщения с объектом
        /// </summary>
        /// <param name="str"></param>
        /// <param name="err"></param>
        public void WriteLn(Object str, Type type, Object obj)
        {

            var st = getStrObject(type, obj);

            StringBuilder sb = GetStr(str);
            if (st != null)
            {
                sb.Append(" :: ");
                sb.Append(st);
            }

            GetCurrenLog().Info(sb.ToString());


            Console.WriteLine(".." + sb.ToString());
        }

        /// <summary>
        /// Используется для записи объекта в лог
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string getStrObject(Type type, object obj)
        {
            string st = null;
            try
            {
                var stream = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(type);


                ser.Serialize(stream, obj);
                stream.Position = 0;
                var sr = new StreamReader(stream);

                st = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                WriteLn("Ошибка при записи", ex);
            }

            return st;
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
