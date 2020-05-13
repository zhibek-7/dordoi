using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Utilities.Logs;

namespace External.Migration
{
    /// <summary>
    /// Класс, предназначенный для миграции данных
    /// </summary>
    public class XliffReader : DataReader
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
        public XliffReader()
        {

        }

        public void Initialize(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'xliff'
        /// </summary>
        /// <param name="fs">Поток xliff-файла миграции</param>
        public void Load(FileStream fs)
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
                File.Delete(tempFileName);
                _logger.WriteLn("xliff-файл успешно распарсен");
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(XliffReader)}.{nameof(Load)}", ex);
            }
        }

    }
}
