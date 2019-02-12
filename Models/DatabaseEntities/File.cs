using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class File : BaseEntity
    {
        public string name_text { get; set; }
        public string description { get; set; }
        public DateTime date_of_change { get; set; }

        public int? strings_count { get; set; }
        public int? version { get; set; }
        public bool? is_last_version { get; set; }
        public int? priority { get; set; }

        public string encod { get; set; }
        public string original_full_text { get; set; }

        public bool is_folder { get; set; }

        public int id_localization_project { get; set; }
        public int? id_folder_owner { get; set; }
        public int? id_previous_version { get; set; }
        public string translator_name { get; set; }
        public string download_name { get; set; }
    }
}
