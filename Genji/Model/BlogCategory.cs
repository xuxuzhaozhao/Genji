using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Genji.Model
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public int Parent { get; set; } = 0;
        public string Name { get; set; }
    }
}
