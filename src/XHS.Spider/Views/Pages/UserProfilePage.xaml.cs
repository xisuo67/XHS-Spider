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
            if (sender is not System.Windows.Controls.CheckBox checkbox)
                return;
            var tag = checkbox.Tag as string;
            if (tag == null) return;
            DataGrid dataGrid = null;
            switch (tag)
            {
                case "UserPosted":
                    dataGrid = this.dgrdView;
                    break;
                case "Collect":
                    dataGrid = this.dgrdViewCollect;
                    break;
                case "Like":
                    dataGrid = this.dgrdViewLike;
                    break;
                default:
                    break;
            }
            if (dataGrid != null)
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    var cntr = dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dataGrid.Columns[0].GetCellContent(ObjROw);
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
