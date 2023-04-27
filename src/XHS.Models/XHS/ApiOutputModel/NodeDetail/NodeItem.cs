using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.NodeDetail
{
    public class NodeItem
    {
        [JsonProperty("")]
        public string id { get; set; }
        [JsonProperty("")]
        public string model_type { get; set; }
        [JsonProperty("")]
        public NoteCardModel note_card { get; set; }
    }
}
