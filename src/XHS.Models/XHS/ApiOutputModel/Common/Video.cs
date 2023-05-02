using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.Common
{
    public class Video
    {
        [JsonProperty("capa")]
        public Capa Capa { get; set; }
        [JsonProperty("consumer")]
        public Consumer Consumer { get; set; }
        [JsonProperty("image")]
        public Image Image { get; set; }
    }
    public class Capa
    {
        public double duration { get; set; }
    }
    public class Consumer
    {
        [JsonProperty("origin_video_key")]
        public string OriginVideoKey { get; set; }
    }

    public class Image
    {
        [JsonProperty("first_frame_fileid")]
        public string FirstFrameFileid { get; set; }
        [JsonProperty("thumbnail_fileid")]
        public string ThumbnailFileid { get; set; }
    }
}
