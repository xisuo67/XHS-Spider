using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private bool _dataInitialized = false;
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
            if (!_dataInitialized)
                InitializeData();
        }
        private void InitializeData() {
            DataGridItemCollection = GlobalCaChe.Cookies.ToArray();
        }
    }
}
