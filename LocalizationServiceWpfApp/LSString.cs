using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationServiceWpfApp
{
    [Table("Strings", Schema = "public")]
    public class LSString
    {
        public long ID { get; set; }
        public bool HasTranslationSubstring { get; set; }
        public string SubstringToTranslate { get; set; }
        public string Description { get; set; }
        public string Context { get; set; }
        public int? TranslationMaxLength { get; set; }
        public int ID_FileOwner { get; set; }
        public int PositionInFile { get; set; }
        public string OriginalString { get; set; }
        public string TranslationSubstring { get; set; }
        public int? TranslationSubstringPositionInLine { get; set; }

        public string State
        {
            get
            {
                if (this.HasTranslationSubstring)
                {
                    return string.IsNullOrEmpty(this.TranslationSubstring) ? "IsNotTranslated" : "Translated";
                }
                else return "HasNotTranslationSubstring";
            }
        }


        public LSString() { }

        /// <summary>
        /// Конструктор для строки, не содержащей подстроку для перевода
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="description">Описание строки</param>
        /// <param name="id_FileOwner">ID файла-владельца строки</param>
        /// <param name="positionInFile">Позиция строки в файле</param>
        /// <param name="originalString">Оригинал строки</param>
        public LSString(db_Entities context, string description, int id_FileOwner, int positionInFile, string originalString)
        {
            this.HasTranslationSubstring = false;
            this.SubstringToTranslate = null;
            this.Description = description;
            this.Context = null;
            this.TranslationMaxLength = null;
            this.ID_FileOwner = id_FileOwner;
            this.PositionInFile = positionInFile;
            this.OriginalString = originalString;
            this.TranslationSubstring = null;
            this.TranslationSubstringPositionInLine = null;
            context.LSString.Add(this);
        }

        /// <summary>
        /// Конструктор для строки, содержащей подстроку для перевода
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="substringToTranslate">Подстрока для перевода</param>
        /// <param name="description">Описание строки</param>
        /// <param name="stringContext">Контекст для перевода</param>
        /// <param name="translationMaxLength">Максимальная длина перевода</param>
        /// <param name="id_FileOwner">ID файла-владельца строки</param>
        /// <param name="positionInFile">Позиция строки в файле</param>
        /// <param name="originalString">Оригинал строки</param>
        /// <param name="translationSubstring">Подстрока перевода</param>
        /// <param name="translationSubstringPositionInLine">Позиция подстроки перевода, в строке-владельце</param>
        public LSString(db_Entities context, string substringToTranslate, string description, string stringContext, int? translationMaxLength, int id_FileOwner, int positionInFile, string originalString, string translationSubstring, int translationSubstringPositionInLine)
        {
            //this.ID = context.LSString.Count() > 0 ? context.LSString.Select(lss => lss.ID).Max() + 1 : 0;
            this.HasTranslationSubstring = true;
            this.SubstringToTranslate = substringToTranslate;
            this.Description = description;
            this.Context = stringContext;
            this.TranslationMaxLength = translationMaxLength;
            this.ID_FileOwner = id_FileOwner;
            this.PositionInFile = positionInFile;
            this.OriginalString = originalString;
            this.TranslationSubstring = translationSubstring;
            this.TranslationSubstringPositionInLine = translationSubstringPositionInLine;
            context.LSString.Add(this);
        }
    }
}
