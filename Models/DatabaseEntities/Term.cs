namespace Models.DatabaseEntities
{
    public class Term : DatabaseEntities.TranslationSubstring
    {

        public int? Part_Of_Speech_Id { get; set; }

        public bool Is_Editable { get; set; }

    }
}
