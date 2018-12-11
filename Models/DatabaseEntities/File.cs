﻿using System;

namespace Models.DatabaseEntities
{
    public class File : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfChange { get; set; }

        public int? StringsCount { get; set; }
        public int? Version { get; set; }
        public int? Priority { get; set; }

        public string Encoding { get; set; }
        public string OriginalFullText { get; set; }

        public bool IsFolder { get; set; }

        public int ID_LocalizationProject { get; set; }
        public int? ID_FolderOwner { get; set; }
    }
}