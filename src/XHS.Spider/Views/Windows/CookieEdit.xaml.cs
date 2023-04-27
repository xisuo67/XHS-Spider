using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.Common.Global;
using XHS.Models.SettingCookie;

namespace XHS.Spider.Views.Windows
{
    /// <summary>
    /// CookieEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CookieEdit : UiWindow, INavigationWindow
    {
        private CookieModel _cookieModel;
        public CookieEdit()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
            InitializeComponent();
        }
        public CookieEdit(CookieModel cookie=null):this() {
            if (cookie!=null)
            {
                _cookieModel=cookie;
                
                this.txtCookie.Text = cookie.Cookie;
                this.txtCookie.Focus();
            }
        }

        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public Frame GetFrame()
        {
            throw new NotImplementedException();
        }

        public INavigation GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow()
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cookie= txtCookie.Text.Trim();
            if (_cookieModel != null) {
                _cookieModel.Cookie = cookie;
            }
            else
            {
                _cookieModel = new CookieModel()
                {
                    Id = Guid.NewGuid(),
                    Cookie = cookie
                };
                GlobalCaChe.Cookies.Add(_cookieModel);
            }
            this.Close();
        }
    }
}
