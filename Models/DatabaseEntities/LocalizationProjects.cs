using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Проект локализации
    /// </summary>
    [Serializable]
    public class LocalizationProject : BaseEntity
    {
        [Required]
        public string Name_text { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string URL { get; set; }

        public bool Visibility { get; set; }
        [Required]
        public DateTime Date_Of_Creation { get; set; }
        [Required]
        public DateTime Last_Activity { get; set; }
        [Required]
        public int ID_Source_Locale { get; set; }

        /// <summary>
        /// Исходный язык
        /// </summary>
        public string Source_Locale_Name { get; set; }
        /// <summary>
        /// Количество активных пользователей
        /// </summary>
        public int? count_users_active { get; set; }


        public bool Able_To_Download { get; set; }

        public bool AbleTo_Left_Errors { get; set; }

        public string Default_String { get; set; }

        public bool Notify_New { get; set; }

        public bool Notify_Finish { get; set; }

        public bool Notify_Confirm { get; set; }

        public byte[] Logo { get; set; }

        public bool notify_new_comment { get; set; }

        public bool original_if_string_is_not_translated { get; set; }

        public bool export_only_approved_translations { get; set; }
    }
}
