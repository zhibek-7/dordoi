using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Term : DatabaseEntities.TranslationSubstring
    {

        public Guid? part_of_speech_id { get; set; }

        public bool is_editable { get; set; }

    }
}
