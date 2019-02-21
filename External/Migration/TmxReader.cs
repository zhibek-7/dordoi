using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Utilities.Logs;

namespace External.Migration
{
    /// <summary>
    /// Класс, предназначенный для считывания данных из tmx
    /// </summary>
    class TmxReader
    {
        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();


        /// <summary>
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public TmxReader()
        {

        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tmx'
        /// </summary>
        /// <param name="fs">Поток tmx-файла миграции</param>
        public void Read(FileStream fs)
        {
            try
            {
                _logger.WriteLn("Распарсивание tmx-файла");
                var tempFileName = Path.GetTempFileName();
                fs.Seek(0, SeekOrigin.Begin);
                fs.CopyTo(File.Open(tempFileName, FileMode.Open));
                var d = XDocument.Load(tempFileName);
                var srclang = d.Root.Element("header").Attribute("srclang");
                var body = d.Root.Element("body");
                var ns_xml = body.GetNamespaceOfPrefix("xml");
                foreach (var tu in body.Elements("tu"))
                {
                    foreach (var tuv in tu.Elements("tuv"))
                    {
                        var lang = tuv.Attribute(ns_xml + "lang").Value;
                        var trans = tuv.Element("seg").Value;
                    }
                }
                File.Delete(tempFileName);
                _logger.WriteLn("tmx-файл успешно распарсен");
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(TmxReader)}.{nameof(Read)}", ex);
            }
        }
    }
}
