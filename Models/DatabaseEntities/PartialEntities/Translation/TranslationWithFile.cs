using System;

namespace Models.DatabaseEntities.PartialEntities.Translations
{
    [Serializable]
    public class TranslationWithFile
    {
        /// <summary>
        /// Файл, которому принадлежит вариант перевода
        /// </summary>
        public string file_owner_name { get; set; }

        /// <summary>
        /// Текст для перевода
        /// </summary>
        public string translation_text { get; set; }

        /// <summary>
        /// Вариант перевода
        /// </summary>
        public string translation_variant { get; set; }
    }
}
