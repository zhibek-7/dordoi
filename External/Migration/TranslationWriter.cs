using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Utilities;
using Utilities.Logs;

namespace External.Migration
{
    public class TranslationWriter
    {


        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();

        private TranslationRepository translationRep;
        private LocaleRepository localeRep;

        private TranslationSubstringRepository ts;

        private string _localDefault;

        public TranslationWriter(string localDefault)
        {
            _localDefault = localDefault;
            var connectionString = Settings.GetStringDB();
            translationRep = new TranslationRepository(connectionString);
            localeRep = new LocaleRepository(connectionString);

            ts = new TranslationSubstringRepository(connectionString);
        }



        public void AppendTable(Hashtable ht, string lang, string term)
        {
            if (ht.Contains(lang))
            {
                _loggerError.WriteLn(
                    $"Ошибка в {typeof(TranslationWriter)}.{nameof(AppendTable)} Два перевода одного термина [" + term + "] на один язык [" + lang + "]");
            }
            else
            {
                ht.Add(lang, term);
            }
        }

        public TranslationSubstring AddTranslationSubstring(string lang, Hashtable ht, Guid? newTermId, Guid glossaryId, Guid idFile)
        {
            if (lang.Equals(_localDefault))
            {
                TranslationSubstring newTerm =
                    new TranslationSubstring(ht[_localDefault].ToString(), null, idFile, null, 0);
                //newTermId = glossaryRep.AddNewTermAsync(glossaryId, newTerm, null).Result;

                // newTermId = ts.AddAsync(newTerm).Result;
                return newTerm;
            }

            return null;
        }


        public void AddTranslation(object lang, Guid? newTermId, Hashtable ht)
        {


            if (lang.Equals(_localDefault) == false && newTermId != null)
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
                    _loggerError.WriteLn($"Ошибка в {typeof(TranslationWriter)}.{nameof(AddTranslation)} " + error, ex);
                }
            }
        }

        public string CopyFile(FileStream fs)
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
