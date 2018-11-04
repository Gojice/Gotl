using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatling
{
    public class MediaBox
    {
        public string kind { set; get; }
        public string Video { get; set; }
        public string Picture { get; set; }
        public string caption { get; set; }
        public string location { get; set; }
        public string[] GVideo { get; set; }
        public string[] GPicture { get; set; }
    }
}
