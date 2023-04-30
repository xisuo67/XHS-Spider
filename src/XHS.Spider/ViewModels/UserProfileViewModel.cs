using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using XHS.IService.XHS;
using XHS.Spider.Services;

namespace XHS.Spider.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject, INavigationAware
    {
        public static readonly string BaseUrl = "https://www.xiaohongshu.com/user/profile/";
        private string inputText;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }
        private readonly ISnackbarService _snackbarService;
        private readonly IXhsSpiderService _xhsSpiderService;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPageServiceNew _pageServiceNew;
        public UserProfileViewModel(IServiceProvider serviceProvider, IPageServiceNew pageServiceNew, ISnackbarService snackbarService, IXhsSpiderService xhsSpiderService)
        {
  
            _snackbarService = snackbarService;
            _xhsSpiderService = xhsSpiderService;
        }


        // 输入确认事件
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get => inputCommand ?? (inputCommand = new RelayCommand(ExecuteInput));
            set => inputCommand = value;
        }
        /// <summary>
        /// 处理输入事件
        /// </summary>
        private void ExecuteInput()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (InputText.Contains("user/profile/"))
                {
                   var id= SearchService.GetId(inputText,BaseUrl);
                    if (string.IsNullOrEmpty(id)) {
                        return;
                    }
                    else
                    {
                        var apiResult= _xhsSpiderService.GetOtherInfo(id);
                        if (apiResult != null && apiResult.Success)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }
            //TODO:
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {

        }
    }
}
