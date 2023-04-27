using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.OtherInfo
{
    public class BasicInfo
    {
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("red_id")]
        public string RedId { get; set; }
        [JsonProperty("gender")]
        public int Gender { get; set; }
        [JsonProperty("ip_location")]
        public string IpLocation { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("imageb")]
        public string Imageb { get; set; }
        [JsonProperty("nickname")]
        public string NickName { get; set; }
    }
}
