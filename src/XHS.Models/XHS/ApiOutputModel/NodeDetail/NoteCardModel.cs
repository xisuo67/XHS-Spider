using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.Common;
using XHS.Models.XHS.ApiOutputModel.UserPosted;

namespace XHS.Models.XHS.ApiOutputModel.NodeDetail
{
    public class NoteCardModel
    {
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("last_update_time")]
        public long LastUpdateTime { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("user")]
        public NodeUser User { get; set; }

        [JsonProperty("interact_info")]
        public InteractInfoModel InteractInfo { get; set; }
        [JsonProperty("image_list")]
        public List<ImageListModel> ImageList { get; set; }
        [JsonProperty("tag_list")]
        public List<TagListModel> TagList { get; set; }
        [JsonProperty("note_id")]
        public string NoteId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

         //"at_user_list": [],
         //           "share_info": {
         //               "un_share": false
         //           }
         //这俩参数不要了
    }
}
