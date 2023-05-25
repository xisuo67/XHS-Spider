using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;

namespace XHS.Models.XHS.ApiOutputModel.Search
{
    public class SearchNodeItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("model_type")]
        public string ModelType { get; set; }
        [JsonProperty("note_card")]
        public NoteCardSearchModel NoteCard { get; set; }
    }
}
