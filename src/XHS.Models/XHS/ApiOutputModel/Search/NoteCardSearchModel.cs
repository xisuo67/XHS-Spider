using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.Common;
using XHS.Models.XHS.ApiOutputModel.UserPosted;

namespace XHS.Models.XHS.ApiOutputModel.Search
{
    public class NoteCardSearchModel: BaseNoteCardModel
    {
        [JsonProperty("cover")]
        public NodeCover Cover { get; set; }
    }
}
