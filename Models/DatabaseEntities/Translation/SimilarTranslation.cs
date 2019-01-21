namespace Models.DatabaseEntities.Translations
{
    public class SimilarTranslation : TranslationWithFile
    {
        /// <summary>
        /// Процент схожести
        /// </summary>
        public double Similarity { get; set; }
    }
}
