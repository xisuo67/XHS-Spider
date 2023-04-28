using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;

namespace XHS.Spider.Services
{
    public class SearchService
    {
        /// <summary>
        /// 解析支持的输入
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void SearchInput(string input, INavigation navigation)
        {
            //测试跳转
            navigation.Navigate(typeof(Views.Pages.UserProfilePage), "test");
           
        }
    }
}
