using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using Wpf.Ui.Common.Interfaces;
using XHS.Common.Global;
using XHS.Models.SettingCookie;

namespace XHS.Spider.ViewModels
{
    /// <summary>
    /// Cookie设置
    /// </summary>
    public partial class SettingCookieViewModel : ObservableObject, INavigationAware
    {
        private IEnumerable<CookieModel> _dataGridItemCollection = new CookieModel[] { };
        public IEnumerable<CookieModel> DataGridItemCollection
        {
            get => _dataGridItemCollection;
            set => SetProperty(ref _dataGridItemCollection, value);
        }
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            InitializeData();
        }
        public void InitializeData()
        {
            DataGridItemCollection = GlobalCaChe.Cookies.ToArray();
        }
    }
}
