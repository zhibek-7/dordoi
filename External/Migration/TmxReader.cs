using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Utilities;
using Utilities.Logs;

namespace External.Migration
{
    /// <summary>
    /// Класс, предназначенный для считывания данных из tmx
    /// </summary>
    class TmxReader : DataReader
    {
        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();


        private TranslationMemoryRepository gsr;
        private TranslationMemory memory;
        private GlossaryRepository glossaryRep;

        private TranslationRepository translationRep;
        private LocaleRepository localeRep;
        private Guid memoryId;

        /// <summary>
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public TmxReader()
        {

        }

        public void Initialize()
        {

            var connectionString = Settings.GetStringDB();
            //TOD нужно передавать глассарий
            memoryId = Guid.Parse("4f26d743-e5c4-4887-84b7-7c4493611b28");
            gsr = new TranslationMemoryRepository(connectionString);
            IEnumerable<TranslationMemory> gList = gsr.GetForEditAsync(memoryId).Result;
            List<TranslationMemory> tt = gList.ToList();
            memory = tt[0];
            //TODO нужно обработать создание нового глоссария, если текущего нет

            glossaryRep = new GlossaryRepository(connectionString);
            translationRep = new TranslationRepository(connectionString);
            localeRep = new LocaleRepository(connectionString);
        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tmx'
        /// Translation Memory Exchange Format — Обмен памятью переводов
        /// </summary>
        /// <param name="fs">Поток tmx-файла миграции</param>
        public void Load(FileStream fs)
        {
            try
            {
                _logger.WriteLn("Распарсивание tmx-файла");
                var tempFileName = Path.GetTempFileName();
                fs.Seek(0, SeekOrigin.Begin);
                fs.CopyTo(System.IO.File.Open(tempFileName, FileMode.Open));
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
                System.IO.File.Delete(tempFileName);
                _logger.WriteLn("tmx-файл успешно распарсен");
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(TmxReader)}.{nameof(Load)}", ex);
            }
        }
    }
}
