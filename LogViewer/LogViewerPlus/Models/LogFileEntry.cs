using LogViewerPlus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerPlus.Models
{
    public class LogFileEntry
    {
        public string Message { get; set; }
        public string  ShortMessage { get; set; }

        public DateTime Time { get; set; }
        public string  ShortTime { get; set; }

        public string ErrorTrance { get; set; }

        public MessageType MessageType { get; set; }

    }
}
