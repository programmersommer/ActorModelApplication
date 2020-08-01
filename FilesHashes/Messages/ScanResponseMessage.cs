using System.Collections.Generic;

namespace FilesHashes.Messages
{
    public class ScanResponseMessage
    {
        public Dictionary<string, string> Files { get; }
        public string File { get; set; }
    }
}
