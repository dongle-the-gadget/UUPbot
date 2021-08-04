using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnofficialUUPDumpBot
{
    public class UupItem
    {
        public string title { get; set; }
        public string build { get; set; }
        public string arch { get; set; }
        public string uuid { get; set; }
    }

    public class Response
    {
        public Dictionary<string, UupItem> builds { get; set; }
    }

    public class UUPDumpRes
    {
        public Response response { get; set; }
    }
}
