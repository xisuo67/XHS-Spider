using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.ApiOutputModel.UserPosted
{
    /// <summary>
    /// 用户笔记
    /// </summary>
    public class UserPostedModel
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("cursor")]
        public string Cursor { get; set; }

        [JsonProperty("notes")]
        public List<NoteModel> Notes { get; set; }


    }
}
