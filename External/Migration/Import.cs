using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Utilities.Logs;

namespace External.Migration
{
    public class Import
    {
        private DataReader reader;

        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();


        public enum FileType
        {
            TBX,
            TMX,
            XLIFF
        }



        /**
         * Импорт
         */
        public void imp(FileType ti, FileStream fs, Guid id)
        {

            try
            {
                switch (ti)
                {
                    case FileType.TBX:
                        this.reader = new TbxReader();
                        break;
                    case FileType.TMX:
                        this.reader = new TmxReader();
                        break;
                    case FileType.XLIFF:
                        this.reader = new XliffReader();
                        break;
                }

                reader.Initialize(id);
                reader.Load(fs);

            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(Import)}.{nameof(imp)}", ex);
            }
        }
    }
}
