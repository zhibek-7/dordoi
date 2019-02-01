using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Utilities.Logs;

namespace TestParseConsoleApp
{
    /// <summary>
    /// Класс, предназначенный для миграции данных из CrowdIn
    /// </summary>
    class CrowdInMigration
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
        public CrowdInMigration()
        {

        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'xliff'
        /// </summary>
        /// <param name="fs">Поток xliff-файла миграции</param>
        public void LoadXliff(FileStream fs)
        {
            try
            {
                _logger.WriteLn("Распарсивание xliff-файла");
                var tempFileName = Path.GetTempFileName();
                fs.Seek(0, SeekOrigin.Begin);
                fs.CopyTo(File.Open(tempFileName, FileMode.Open));
                var d = XDocument.Load(tempFileName);
                var ns = d.Root.GetDefaultNamespace();
                foreach (var file in d.Root.Elements(ns + "file"))
                {
                    string original = file.Attribute("original").Value;
                    //here we need to check if filename (original variable) exists
                    string source_language = file.Attribute("source-language").Value;
                    string target_language = file.Attribute("target-language").Value;
                    var body = file.Element(ns + "body");
                    foreach (var el in body.Elements())
                    {
                        switch (el.Name.LocalName)
                        {
                            case "trans-unit":
                                {
                                    string source = el.Element(ns + "source").Value;
                                    string target = el.Element(ns + "target").Value;
                                    string note = el.Element(ns + "note").Value;
                                    //ok, now it's all about where to write gathered data
                                    break;
                                }
                        }
                    }
                }
                _logger.WriteLn("xliff-файл успешно распарсен");
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(CrowdInMigration)}.{nameof(LoadXliff)}", ex);
            }
        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tbx'
        /// </summary>
        /// <param name="fs">Поток tbx-файла миграции</param>
        public void LoadTbx(FileStream fs)
        {
            try
            {
                _logger.WriteLn("Распарсивание tbx-файла");
                var tempFileName = Path.GetTempFileName();
                fs.Seek(0, SeekOrigin.Begin);
                fs.CopyTo(File.Open(tempFileName, FileMode.Open));
                var d = XDocument.Load(tempFileName);
                var ns = d.Root.GetNamespaceOfPrefix("xml");
                foreach (var text in d.Root.Elements("text"))
                {
                    foreach (var termEntry in text.Element("body").Elements("termEntry"))
                    {
                        foreach (var langSet in termEntry.Elements("langSet"))
                        {
                            string lang = langSet.Attribute(ns + "lang").Value;
                            string term = langSet.Element("tig").Element("term").Value;
                            //ok, now it's all about where to write gathered data
                        }
                    }
                }
                _logger.WriteLn("tbx-файл успешно распарсен");
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(CrowdInMigration)}.{nameof(LoadTbx)}", ex);
            }
        }
    }
}
