using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class FolderModel
    {
        public string name { get; set; }
        public Guid projectId { get; set; }
        public Guid? parentId { get; set; }

    }
}
