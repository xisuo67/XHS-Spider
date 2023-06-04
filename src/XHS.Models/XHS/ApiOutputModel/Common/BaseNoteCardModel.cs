using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;
using XHS.Models.XHS.ApiOutputModel.UserPosted;

namespace XHS.Models.XHS.ApiOutputModel.Common
{
    public class BaseNoteCardModel
    {
        [JsonProperty("interact_info")]
        public InteractInfoModel InteractInfo { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("user")]
        public NodeUser User { get; set; }
    }
}
