namespace Models.DatabaseEntities
{
    public class TranslationSubstring : BaseEntity
    {
        public string SubstringToTranslate { get; set; }

        public string Description { get; set; }

        public string Context { get; set; }

        public int? TranslationMaxLength { get; set; }

        public int ID_FileOwner { get; set; }

        public string Value { get; set; }

        public int PositionInText { get; set; }

        public bool? Outdated { get; set; }

        public TranslationSubstring() { }

        public TranslationSubstring(string substringToTranslate, string context, int id_FileOwner, string value, int positionInText)
        {
            this.SubstringToTranslate = substringToTranslate;
            this.Description = null;
            this.Context = context;
            this.TranslationMaxLength = null;
            this.ID_FileOwner = id_FileOwner;
            this.Value = value;
            this.PositionInText = positionInText;
        }
    }
}
