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
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("model_type")]
        public string ModelType { get; set; }
        [JsonProperty("note_card")]
        public NoteCardModel NoteCard { get; set; }
    }
}
