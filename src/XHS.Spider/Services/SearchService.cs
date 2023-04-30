using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Services
{
    public class SearchService
    {
        /// <summary>
        /// 解析支持的输入
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void SearchInput(string input, INavigation navigation, IServiceProvider serviceProvider, IPageServiceNew pageServiceNew)
        {
           
            if (input.Contains("user/profile"))
            {
                SetJumpParam(input, serviceProvider,pageServiceNew);
                navigation.Navigate(typeof(Views.Pages.UserProfilePage));
            }
        }
        private static void SetJumpParam(string input, IServiceProvider serviceProvider, IPageServiceNew pageServiceNew)
        {
            pageServiceNew.Scope = serviceProvider.CreateScope();
            var dc = pageServiceNew.Scope.ServiceProvider.GetRequiredService<UserProfileViewModel>();
            dc.InputText = input;
        }
        /// <summary>
        /// 从url中获取id
        /// </summary>
        /// <param name="input"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static string GetId(string input, string baseUrl)
        {
            if (!IsUrl(input)) { return ""; }

            string url = EnableHttps(input);
            url = DeleteUrlParam(url);
            return url.Replace(baseUrl, "");
        }

        /// <summary>
        /// 是否为网址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsUrl(string input)
        {
            return input.StartsWith("http://") || input.StartsWith("https://");
        }

        /// <summary>
        /// 将http转为https
        /// </summary>
        /// <returns></returns>
        private static string EnableHttps(string url)
        {
            if (!IsUrl(url)) { return null; }

            return url.Replace("http://", "https://");
        }

        /// <summary>
        /// 去除url中的参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string DeleteUrlParam(string url)
        {
            string[] strList = url.Split('?');

            return strList[0].EndsWith("/") ? strList[0].TrimEnd('/') : strList[0];
        }
    }
}
