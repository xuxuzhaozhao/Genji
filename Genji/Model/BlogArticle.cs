using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Genji.Model
{
    public class BlogArticle
    {
        public int Id { get; set; }
        public string Image { get; set; } = "http://img.xuxuzhaozhao.top/20180307/144311_520.png";
        public string Title { get; set; }
        public int Category { get; set; }
        public string CategoryName { get; set; }
        public string Tag { get; set; } = "";
        public string Content { get; set; }
        public int ReadCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public bool IsPublished { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
        public DateTime DateTime { get; set; }
    }
}
