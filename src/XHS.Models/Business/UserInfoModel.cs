using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XHS.Models.Business
{
    public class UserInfoModel
    {
        public string UserName { get; set; }

        public string HeadUrl { get; set; }

        public BitmapImage HeadImage { get; set; }
    }
}
