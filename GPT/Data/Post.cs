using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPT.Data
{
    public class Post
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public bool Published { get; set; }
        //public string Status { get; set; }
    }
    class PostStatus : Post
    {
        public string Status { get; set; }
    }
}
