using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.OtherInfo
{
    public class Tag
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tagType")]
        public string TagType { get; set; }
    }
}
