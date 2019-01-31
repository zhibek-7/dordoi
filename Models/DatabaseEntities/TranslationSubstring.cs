namespace Models.DatabaseEntities
{
    public class TranslationSubstring : BaseEntity
    {
        public string Substring_To_Translate { get; set; }

        public string Description { get; set; }

        public string Context { get; set; }

        public int? Translation_Max_Length { get; set; }

        public int ID_File_Owner { get; set; }

        public string Value { get; set; }

        public int Position_In_Text { get; set; }

        public bool? Outdated { get; set; }

        public TranslationSubstring() { }

        public TranslationSubstring(string substringToTranslate, string context, int id_FileOwner, string value, int positionInText)
        {
            this.Substring_To_Translate = substringToTranslate;
            this.Description = null;
            this.Context = context;
            this.Translation_Max_Length = null;
            this.ID_File_Owner = id_FileOwner;
            this.Value = value;
            this.Position_In_Text = positionInText;
        }
    }
}
