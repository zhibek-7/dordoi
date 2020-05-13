using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.PartialEntities.Translation
{
    [Serializable]
    public class TranslationWithLocaleText
    {
        /// <summary>
        /// Текст для перевода
        /// </summary>
        public string translation_variant { get; set; }

        /// <summary>
        /// Название языка перевода
        /// </summary>
        public string locale_name_text { get; set; }
    }
}
