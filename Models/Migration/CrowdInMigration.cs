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
        /// Поле, предназначенное для логирования класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();


        /// <summary>
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public CrowdInMigration()
        {

        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'xliff'
        /// </summary>
        /// <param name="filename">Полное имя файла миграции</param>
        public void LoadXliff(string filename)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'xliff'", filename));
            var d = XDocument.Load(filename);
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
            _logger.WriteLn(string.Format("Файл {0} успешно распарсен", filename));
        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tbx'
        /// </summary>
        /// <param name="filename">Полное имя файла миграции</param>
        public void LoadTbx(string filename)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'tbx'", filename));
            var d = XDocument.Load(filename);
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
            _logger.WriteLn(string.Format("Файл {0} успешно распарсен", filename));
        }
    }
}
