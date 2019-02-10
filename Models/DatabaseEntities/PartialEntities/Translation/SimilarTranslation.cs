using System;

namespace Models.DatabaseEntities.PartialEntities.Translations
{
    [Serializable]
    public class SimilarTranslation : TranslationWithFile
    {
        /// <summary>
        /// Процент схожести
        /// </summary>
        public double Similarity { get; set; }
    }
}
