using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using XHS.Common.Helpers;
using XHS.Models.Enum;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// UserProfilePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserProfilePage : INavigableView<UserProfileViewModel>
    {
        public UserProfileViewModel ViewModel
        {
            get;
        }

        public UserProfilePage(UserProfileViewModel viewModel)
        {
            viewModel.InitNullImage();
            ViewModel = viewModel;
            InitializeComponent();
        }
        private void CheckAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.dgrdView.Items.Count; i++)
            {
                var cntr = dgrdView.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dgrdView.Columns[0].GetCellContent(ObjROw);
                    if (objElement != null)
                    {
                        System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
                        if (objChk.IsChecked == false)
                        {
                            objChk.IsChecked = true;
                        }
                        else
                        {
                            objChk.IsChecked = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Tab切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            { 
                var tabControl= sender as TabControl;
                var data = tabControl.SelectedItem as TabItem;
                var tag = data.Tag;
                if (tag != null)
                {
                    //TODO:切换tab时触发
                    var tagName= tag?.ToString();
                    var noteTypeEnum = MemberInfoExtensions.StringToEnum<NoteTypeEnum>(tagName);
                    ViewModel.OnNavigateTab(noteTypeEnum);
                }
            }
        }
    }
}
