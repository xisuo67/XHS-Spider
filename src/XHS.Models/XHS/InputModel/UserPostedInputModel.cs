using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Models.XHS.InputModel
{
    public class UserPostedInputModel
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }

        public string cursor { get; set; } = "";

        public string user_id { get; set; }
    }
}
