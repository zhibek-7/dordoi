using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Модель для записи действий пользователей в системе
    /// </summary>
    [Serializable]
    public class UserAction : BaseEntity
    {
        [Required]
        public Guid id_user { get; set; }

        public string user_name { get; set; }

        [Required]
        public int id_work_type { get; set; }

        public string work_type_name { get; set; }

        [Required]
        public DateTime datetime { get; set; }

        public string description { get; set; }

        public Guid? id_locale { get; set; }

        public string locale_name { get; set; }

        public Guid? id_file { get; set; }

        public string file_name { get; set; }

        public Guid? id_string { get; set; }

        public string translation_substring_name { get; set; }

        public Guid? id_translation { get; set; }

        public string translation { get; set; }

        public Guid? id_project { get; set; }

        public string project_name { get; set; }

        public Guid? id_user_participant { get; set; }

        public Guid? id_role_participant { get; set; }

        public Guid? id_glossary { get; set; }

        public string glossary_name { get; set; }


        public UserAction() { }

        public UserAction(Guid userId, string userName, string descript, int actionTypeID, string actionName)
        {
            this.id_user = userId;
            this.user_name = userName;
            this.id_work_type = actionTypeID;
            this.work_type_name = actionName;
            this.description = descript;
            this.datetime = DateTime.Now;
        }


    }
}
