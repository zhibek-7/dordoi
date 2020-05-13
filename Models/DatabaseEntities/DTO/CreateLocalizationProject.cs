using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Проект локализации для создания
    /// </summary>
    [Serializable]
    public class CreateLocalizationProject //: BaseEntity
    {
        public string Name_text { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public bool Visibility { get; set; }

        public Guid? ID_Source_Locale { get; set; }
        
        public bool? Able_To_Download { get; set; }

        public bool? Able_To_Left_Errors { get; set; }

        public string Default_String { get; set; }

        public bool? Notify_New { get; set; }

        public bool? Notify_Finish { get; set; }

        public bool? Notify_Confirm { get; set; }

        public byte[] Logo { get; set; }

        public bool? notify_new_comment { get; set; }

        public bool? original_if_string_is_not_translated { get; set; }

        public bool? export_only_approved_translations { get; set; }

        public bool? able_translators_change_terms_in_glossaries { get; set; }
    }
}
