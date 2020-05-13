using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public class TbxReader : DataReader
    {


        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();

        private GlossariesRepository gsr;
        private Glossaries glossaries;
        private Guid glossaryId;

        private string _localDefault = "ru";

        private TranslationWriter tWriter;
        private GlossaryRepository glossaryRep;
        //


        /// <summary>
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public TbxReader()
        {
        }

        public void Initialize(Guid id)
        {

            var connectionString = Settings.GetStringDB();
            //TOD нужно передавать глассарий
            //glossaryId = Guid.Parse("e5f51aa3-8c6b-478f-a277-5497f608eba6");
            glossaryId = id;
            gsr = new GlossariesRepository(connectionString);
            IEnumerable<Glossaries> gList = gsr.GetGlossaryForEditAsync(glossaryId).Result;

            List<Glossaries> tt = gList.ToList();
            glossaries = tt[0];
            //TODO нужно обработать создание нового глоссария, если текущего нет
            tWriter = new TranslationWriter(_localDefault);
            glossaryRep = new GlossaryRepository(connectionString);
            //
        }

        /// <summary>
        /// Функция, предназначенная для распарсивания файла 'tbx'?
        /// Termbase Exchange format — Обмен терминологическими базами
        /// </summary>
        /// <param name="fs">Поток tbx-файла миграции</param>
        public void Load(FileStream fs)
        {
            try
            {

                _logger.WriteLn("Распарсивание tbx-файла");
                var tempFileName = tWriter.CopyFile(fs);

                var d = XDocument.Load(tempFileName);
                var ns = d.Root.GetNamespaceOfPrefix("xml");
                foreach (var text in d.Root.Elements("text"))
                {
                    foreach (var termEntry in text.Element("body").Elements("termEntry"))
                    {
                        Hashtable ht = new Hashtable();
                        Guid? newTermId = null;
                        foreach (var langSet in termEntry.Elements("langSet"))
                        {
                            string lang = langSet.Attribute(ns + "lang").Value;
                            string term = langSet.Element("tig").Element("term").Value;
                            //ok, now it's all about where to write gathered data

                            tWriter.AppendTable(ht, lang, term);
                            TranslationSubstring termString = tWriter.AddTranslationSubstring(lang, ht, newTermId, glossaryId, (Guid)glossaries.ID_File);

                            if (termString != null)
                            {
                                newTermId = glossaryRep.AddNewTermAsync(glossaryId, termString, null).Result;
                            }
                        }

                        foreach (var lang in ht.Keys)
                        {
                            tWriter.AddTranslation(lang, newTermId, ht);
                        }
                        // break;
                    }
                }
                _logger.WriteLn("tbx-файл успешно распарсен");
                System.IO.File.Delete(tempFileName);

                /*
                 *
                 *
                 * INSERT INTO public.translation_substrings_locales
(id_translation_substrings, id_locale)
select id_string, id_locale
from (
select distinct t.id_string as id_string,  id_locale
FROM public.translations as t) as tt



                
INSERT INTO public.files_locales
(id_file, id_locale,percent_of_confirmed, percent_of_translation)
select 'd297766a-8d17-46f4-b8fc-be7e9165c66b', f. id_locale, 0,0 
from (select distinct  id_locale as id_locale
FROM public.translations) as f


                 */
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в {typeof(TbxReader)}.{nameof(Load)}", ex);
            }
        }


    }
}
