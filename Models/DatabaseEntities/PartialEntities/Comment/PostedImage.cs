using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.PartialEntities.Comment
{
    [Serializable]
    public class PostedImage
    {
        public PostedImage(string name, string filename, byte[] file)
        {

        }

        public string Name_text { get; private set; }
        public string File_name { get; private set; }
        public byte[] File { private set; get; }
    }
}
