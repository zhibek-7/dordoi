using System;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Таблица localization_projects_translation_memories.
    /// </summary>
    [Serializable]
    public class ProjectTranslationMemory //: BaseEntity
    {
        public Guid? project_id { get; set; }
        public string project_name_text { get; set; }

        public Guid? translationMemory_id { get; set; }
        public string translationMemory_name_text { get; set; }
    }
}
