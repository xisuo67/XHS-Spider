using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS
{
    public class NodeCover
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
        [JsonProperty("height")]
        public double Height { get; set; }
        [JsonProperty("width")]
        public double Width { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }
    }
}
