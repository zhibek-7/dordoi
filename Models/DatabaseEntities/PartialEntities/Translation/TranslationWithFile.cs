namespace Models.DatabaseEntities.PartialEntities.Translations
{
    public class TranslationWithFile
    {
        /// <summary>
        /// Файл, которому принадлежит вариант перевода
        /// </summary>
        public string FileOwnerName { get; set; }

        /// <summary>
        /// Текст для перевода
        /// </summary>
        public string TranslationText { get; set; }

        /// <summary>
        /// Вариант перевода
        /// </summary>
        public string TranslationVariant { get; set; }
    }
}
