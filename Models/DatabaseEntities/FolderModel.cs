using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class FolderModel
    {
        public string Name_text { get; set; }
        public int? Parent_Id { get; set; }
        public int Project_Id { get; set; }
    }
}
