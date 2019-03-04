using Models.DatabaseEntities;

namespace Models.DatabaseEntities.DTO
{
    public class TranslationSubstringTableViewDTO : BaseEntity
    {
        public string substring_to_translate { get; set; }

        public string translation_memories_name { get; set; }
    }
}
