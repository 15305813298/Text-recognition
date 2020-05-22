using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_recognition.Model
{
    public class returnInfo
    {
        public string log_id { get; set; }
        public string direction { get; set; }
        public int words_result_num { get; set; }
        public string words { get; set; }
        public double variance { get; set; }
        public double averaage { get; set; }
        public double min { get; set; }
    }
}
