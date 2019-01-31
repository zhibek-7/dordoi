namespace Models.DatabaseEntities.PartialEntities.Translations
{
    public class SimilarTranslation : TranslationWithFile
    {
        /// <summary>
        /// Процент схожести
        /// </summary>
        public double Similarity { get; set; }
    }
}
