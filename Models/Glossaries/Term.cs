namespace Models.Glossaries
{
    public class Term : DatabaseEntities.String
    {

        public int? PartOfSpeechId { get; set; }

        public bool IsEditable { get; set; }

    }
}
