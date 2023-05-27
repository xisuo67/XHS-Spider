using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Services
{
    public class RedirectService<T> where T : BaseSearchViewModel
    {
        public static void SetJumpParam(string input, IServiceProvider serviceProvider, IPageServiceNew pageServiceNew, WebView2 webView)
        {
            pageServiceNew.Scope = serviceProvider.CreateScope();
            var dc = pageServiceNew.Scope.ServiceProvider.GetRequiredService<T>();
            dc.InputText = input;
            dc.webView = webView;
            dc.ExecuteInitData();
        }
    }
}
