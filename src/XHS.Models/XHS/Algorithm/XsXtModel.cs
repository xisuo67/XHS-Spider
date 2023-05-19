using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.Algorithm
{
    public class XsXtModel
    {
        [JsonProperty("X-s")]
        public string Xs { get; set; }
        [JsonProperty("X-t")]
        public long Xt { get; set; }
    }
}
