using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class TranslationSubstring : BaseEntity
    {
        public string substring_to_translate { get; set; }

        public string description { get; set; }

        public string context { get; set; }

        public int? translation_max_length { get; set; }

        public int id_file_owner { get; set; }

        public string value { get; set; }

        public int position_in_text { get; set; }

        public bool? outdated { get; set; }

        public TranslationSubstring() { }

        public TranslationSubstring(string substringToTranslate, string context, int id_FileOwner, string value, int positionInText)
        {
            this.substring_to_translate = substringToTranslate;
            this.description = null;
            this.context = context;
            this.translation_max_length = null;
            this.id_file_owner = id_FileOwner;
            this.value = value;
            this.position_in_text = positionInText;
        }
    }
}
