using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.NodeDetail
{
    public class NodeDetailModel: XHSBaseApiModel<NodeDetailModel>
    {
        [JsonProperty("")]
        public string cursor_score { get; set; }


    }
}
