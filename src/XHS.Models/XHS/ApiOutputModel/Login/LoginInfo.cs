using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Login
{
    public class LoginInfo
    {
        [JsonProperty("secure_session")]
        public string SecureSession { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("session")]
        public string Session { get; set; }
    }
}
