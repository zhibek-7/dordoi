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
        private GlossaryRepository glossaryRep;
        private TranslationRepository translationRep;
        private LocaleRepository localeRep;
        private Guid glossaryId;
        //


        /// <summary>
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public TbxReader()
        {
        }

        public void Initialize()
        {

            var connectionString = Settings.GetStringDB();
            //TOD нужно передавать глассарий
            glossaryId = Guid.Parse("e5f51aa3-8c6b-478f-a277-5497f608eba6");
            gsr = new GlossariesRepository(connectionString);
            IEnumerable<Glossaries> gList = gsr.GetGlossaryForEditAsync(glossaryId).Result;

            List<Glossaries> tt = gList.ToList();
            glossaries = tt[0];
            //TODO нужно обработать создание нового глоссария, если текущего нет

            glossaryRep = new GlossaryRepository(connectionString);
            translationRep = new TranslationRepository(connectionString);
            localeRep = new LocaleRepository(connectionString);
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
                var tempFileName = CopyFile(fs);

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

                            if (ht.Contains(lang))
                            {
                                _loggerError.WriteLn($"Ошибка в {typeof(TbxReader)}.{nameof(Load)} Два перевода одного термина {"+term+"} на один язык {"+lang+"}");
                            }
                            else
                            {
                                ht.Add(lang, term);
                            }
                            newTermId = AddTranslationSubstring(lang, ht, glossaries, newTermId, glossaryRep, glossaryId);
                        }


                        if (newTermId != null)
                        {
                            foreach (var lang in ht.Keys)
                            {
                                AddTranslation(lang, newTermId, localeRep, ht, translationRep);
                            }
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

        private static Guid? AddTranslationSubstring(string lang, Hashtable ht, Glossaries glossaries, Guid? newTermId,
            GlossaryRepository glossaryRep, Guid glossaryId)
        {
            if (lang.Equals("ru"))
            {
                TranslationSubstring newTerm =
                    new TranslationSubstring(ht["ru"].ToString(), null, (Guid)glossaries.ID_File, null, 0);
                newTermId = glossaryRep.AddNewTermAsync(glossaryId, newTerm, null).Result;
            }

            return newTermId;
        }

        private void AddTranslation(object lang, Guid? newTermId, LocaleRepository localeRep, Hashtable ht,
            TranslationRepository translationRep)
        {
            if (lang.Equals("ru") == false)
            {
                String error = "";
                try
                {
                    Translation item = new Translation();
                    item.ID_String = (Guid)newTermId;
                    _logger.WriteLn("Язык " + lang.ToString());
                    error = " ERROR: Ошибка в Языке: [" + lang.ToString() + "]";
                    item.ID_Locale = localeRep.GetByCode(lang.ToString()).id;
                    item.Confirmed = false;
                    item.Selected = false;
                    //TODO переделать на реального пользователя
                    item.ID_User = Guid.Parse("1d2d530a-b3eb-45a1-8250-721b3b2237b2");
                    ;
                    item.DateTime = new DateTime();
                    item.Translated = ht[lang].ToString();


                    _logger.WriteLn("Добавление термина ", item.GetType(), item);
                    translationRep.AddAsync(item);
                }
                catch (Exception ex)
                {
                    _loggerError.WriteLn($"Ошибка в {typeof(TbxReader)}.{nameof(Load)} " + error, ex);
                }
            }
        }

        private static string CopyFile(FileStream fs)
        {
            var tempFileName = Path.GetTempFileName();
            fs.Seek(0, SeekOrigin.Begin);
            //fs.CopyTo(File.Open(tempFileName, FileMode.Open));
            using (FileStream file =
                new FileStream(tempFileName,
                    FileMode.OpenOrCreate))
            {
                fs.CopyTo(file);
            }

            return tempFileName;
        }
    }
}
