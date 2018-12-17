using System;
using System.Collections.Generic;
using System.Text;
using Models.Comments;

namespace Models.Comments
{
    public class PostedData
    {
        public PostedData(IDictionary<string, PostedImage> images)
        {
            Images = images;
        }

        public IDictionary<string, PostedImage> Images { get; private set; }
    }
}
