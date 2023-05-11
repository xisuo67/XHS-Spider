using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Common.Utils;
using XHS.Models.SettingCookie;

namespace XHS.Common.Global
{
    public static class GlobalCaChe
    {
        /// <summary>
        /// cookies
        /// </summary>
        public static List<CookieModel> Cookies=new List<CookieModel>();

        public static ClipboardHooker clipboardHooker;
    }
}
