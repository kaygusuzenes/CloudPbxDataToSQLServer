using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToText.Models
{
    class Record
    {
        public Json data { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string calldate { get; set; }
        public string calltype { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public string duration { get; set; }
        public string disposition { get; set; }
        public string queue { get; set; }
        public string record { get; set; }
    }

    public class Json
    {
        public bool success { get; set; }
        public int count { get; set; }
        public List<Datum> data { get; set; }
    }


}
