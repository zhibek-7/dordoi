﻿namespace Models.DatabaseEntities
{
    public class FolderModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int ProjectId { get; set; }
    }
}