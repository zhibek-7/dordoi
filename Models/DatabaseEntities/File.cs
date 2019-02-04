using System;

namespace Models.DatabaseEntities
{
    public class File : BaseEntity
    {
        public string Name_text { get; set; }
        public string Description { get; set; }
        public DateTime? Date_Of_Change { get; set; }

        public int? Strings_Count { get; set; }
        public int? Version { get; set; }
        public bool? Is_Last_Version { get; set; }
        public int? Priority { get; set; }

        public string Encod { get; set; }
        public string Original_Full_Text { get; set; }

        public bool Is_Folder { get; set; }

        public int ID_Localization_Project { get; set; }
        public int? ID_Folder_Owner { get; set; }
        public int? Id_Previous_Version { get; set; }
        public string Translator_Name { get; set; }
        public string Download_Name { get; set; }
    }
}
