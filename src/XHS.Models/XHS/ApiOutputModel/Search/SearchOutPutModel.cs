using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Search
{
    public class SearchOutPutModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("word_request_id")]
        public string WordRequestId { get; set; }
        [JsonProperty("filter_tags")]
        public List<FilterTag> FilterTags { get; set; }
    }
}
