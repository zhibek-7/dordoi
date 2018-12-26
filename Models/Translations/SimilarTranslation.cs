using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Translations
{
    public class SimilarTranslation : TranslationWithFile
    {
        /// <summary>
        /// Процент схожести
        /// </summary>
        public double Similarity { get; set; }
    }
}
