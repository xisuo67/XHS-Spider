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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;
using XHS.Spider.ViewModels;

namespace XHS.Spider.Views.Pages
{
    /// <summary>
    /// NodeDetailPage.xaml 的交互逻辑
    /// </summary>
    public partial class NodeDetailPage : INavigableView<NodeDetailViewModel>
    {
        public NodeDetailViewModel ViewModel
        {
            get;
        }

        public NodeDetailPage(NodeDetailViewModel viewModel)
        {
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
    }
}
