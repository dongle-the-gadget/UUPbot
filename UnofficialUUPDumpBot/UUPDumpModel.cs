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

    public class Response2
    {
        public List<UupItem> builds { get; set; }
    }

    public class UUPDumpRes2
    {
        public Response2 response { get; set; }
    }

    public class UpdateArray3
    {
        public string updateId { get; set; }
        public string updateTitle { get; set; }
        public string foundBuild { get; set; }
        public string arch { get; set; }
    }

    public class Response3
    {
        public List<UpdateArray3> updateArray { get; set; }
    }

    public class UUPDumpRes3
    {
        public Response3 response { get; set; }
    }
}
