using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.OtherInfo
{
    public class OtherInfoModel
    {
        [JsonProperty("basic_info")]
        public BasicInfo BasicInfo { get; set; }
        [JsonProperty("interactions")]
        public List<Interaction> Interactions { get; set; }
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
