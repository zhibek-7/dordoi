using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Comments
{    
    public class CommentWithUserInfo
    {
        public int CommentId { get; set; }

        public string Comment { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
