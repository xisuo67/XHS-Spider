using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.CreateQrCode
{
    public class QrCodeModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("multi_flag")]
        public int MultiFlag { get; set; }
        [JsonProperty("qr_id")]
        public string QrId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
