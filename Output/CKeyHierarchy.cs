using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Output
{
    
        public class CKeyHierarchy
        {
            public string CKey { get; set; }
            public string ParentCKey { get; set; }
            public int ItemType { get; set; }
            public string ItemText { get; set; }
            public bool Required { get; set; }
            public int Depth { get; set; }
            public string PreviousSibling { get; set; }
            public string NextSibling { get; set; }

        }
    
}