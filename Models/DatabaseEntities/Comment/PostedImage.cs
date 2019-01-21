using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.Comment
{
    public class PostedImage
    {
        public PostedImage(string name, string filename, byte[] file)
        {

        }

        public string Name { get; private set; }
        public string Filename { get; private set; }
        public byte[] File { private set; get; }
    }
}
