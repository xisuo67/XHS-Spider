using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XHS.Models.XHS.ApiOutputModel.Me
{
    public class UserInfoModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("red_id")]
        public string RedId { get; set; }
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("imageb")]
        public string Imageb { get; set; }
        [JsonProperty("guest")]
        public bool Guest { get; set; }
        [JsonProperty("gender")]
        public int Gender { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }

        public BitmapImage HeadImage { get; set; }
    }
}
