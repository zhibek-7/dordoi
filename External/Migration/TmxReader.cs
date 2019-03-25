using System;
using System.Collections;
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

        private Guid memoryId;

        private string _localDefault = "ru";

        private TranslationWriter tWriter;
        private TranslationSubstringRepository ts;

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
            memoryId = Guid.Parse("47faa0d8-56da-4e09-b151-ded06b587338");
            gsr = new TranslationMemoryRepository(connectionString);
            IEnumerable<TranslationMemory> gList = gsr.GetForEditAsync(memoryId).Result;
            List<TranslationMemory> tt = gList.ToList();
            memory = tt[0];
            //TODO нужно обработать создание нового глоссария, если текущего нет

            glossaryRep = new GlossaryRepository(connectionString);
            tWriter = new TranslationWriter(_localDefault);
            ts = new TranslationSubstringRepository(connectionString);
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
                //var tempFileName = Path.GetTempFileName();
                //fs.Seek(0, SeekOrigin.Begin);
                //fs.CopyTo(System.IO.File.Open(tempFileName, FileMode.Open));
                var tempFileName = tWriter.CopyFile(fs);
                var d = XDocument.Load(tempFileName);
                var srclang = d.Root.Element("header").Attribute("srclang");
                var body = d.Root.Element("body");
                var ns_xml = body.GetNamespaceOfPrefix("xml");
                foreach (var tu in body.Elements("tu"))
                {
                    Hashtable ht = new Hashtable();
                    Guid? newTermId = null;

                    foreach (var tuv in tu.Elements("tuv"))
                    {
                        var lang = tuv.Attribute(ns_xml + "lang").Value;
                        var trans = tuv.Element("seg").Value;

                        tWriter.AppendTable(ht, lang, trans);
                        //newTermId = tWriter.AddTranslationSubstring(lang, ht, newTermId, memoryId, (Guid)memory.id_file);

                        try
                        {
                            TranslationSubstring termString =
                                tWriter.AddTranslationSubstring(lang, ht, newTermId, memoryId, (Guid)memory.id_file);
                            if (termString != null)
                            {
                                newTermId = ts.AddAsync(termString).Result;
                                gsr.AddTranslationMemoriesStrings((Guid)memoryId, (Guid)newTermId);
                                //newTermId = glossaryRep.AddNewTermAsync(memoryId, termString, null).Result;
                            }
                        }
                        catch (Exception exc)
                        {
                            _logger.WriteLn("ERROR: Перевод [" + trans + "] на язык [" + lang + "] " + exc.Message, exc);
                        }

                    }

                    foreach (var lang in ht.Keys)
                    {
                        tWriter.AddTranslation(lang, newTermId, ht);
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
