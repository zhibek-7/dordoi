using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.PartialEntities.Comment
{
    [Serializable]
    public class PostedData
    {
        public PostedData(IDictionary<string, PostedImage> images)
        {
            Images = images;
        }

        public IDictionary<string, PostedImage> Images { get; private set; }
    }
}
