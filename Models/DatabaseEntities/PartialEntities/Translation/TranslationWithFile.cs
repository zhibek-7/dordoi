namespace Models.DatabaseEntities.PartialEntities.Translations
{
    public class TranslationWithFile
    {
        /// <summary>
        /// Файл, которому принадлежит вариант перевода
        /// </summary>
        public string File_Owner_Name { get; set; }

        /// <summary>
        /// Текст для перевода
        /// </summary>
        public string Translation_Text { get; set; }

        /// <summary>
        /// Вариант перевода
        /// </summary>
        public string Translation_Variant { get; set; }
    }
}
