using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Utilities.Logs;

namespace External.Migration
{
    public class TbxReader
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
        public TbxReader()
        {

        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tbx'?
        /// Termbase Exchange format — Обмен терминологическими базами
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
                File.Delete(tempFileName);
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(TbxReader)}.{nameof(LoadTbx)}", ex);
            }
        }
    }
}
