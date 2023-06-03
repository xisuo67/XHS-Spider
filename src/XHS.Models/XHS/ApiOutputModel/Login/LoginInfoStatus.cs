using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Login
{
    public class LoginInfoStatus
    {
        [JsonProperty("code_status")]
        public int CodeStatus { get; set; }
        [JsonProperty("login_info")]
        public LoginInfo LoginInfo { get; set; }
    }
}
