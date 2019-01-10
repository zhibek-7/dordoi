namespace Models.DatabaseEntities
{
    public class Term : DatabaseEntities.TranslationSubstring
    {

        public int? PartOfSpeechId { get; set; }

        public bool IsEditable { get; set; }

    }
}
