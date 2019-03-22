using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class FilePackage
    {

        [Key]
        public int file_id { get; set; }

        public byte[] data { get; set; }

        public string content_file_name { get; set; }

    }
}
