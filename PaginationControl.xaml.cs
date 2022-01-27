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

namespace DataAnalysisWpf.Controls
{
    public delegate void PageChangedHandle(int pageIndex, int pageSize);
    /// <summary>
    /// PaginationControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaginationControl : UserControl
    {

        public event PageChangedHandle PageChanged;
        Pagination1Model model = new Pagination1Model();

        public PaginationControl()
        {
            InitializeComponent();

            DataContext = model;

            model.currentPageSize = 10;
            model.ChosePageSizes = new List<int> { 10, 20, 30, 50 };
        }


        public static readonly DependencyProperty DataCountProperty = DependencyProperty.Register("DataCount", typeof(int), typeof(PaginationControl), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(DataCountValidation));
        public static bool DataCountValidation(object value)
        {
            return true;
        }
        public int DataCount
        {
            get { return (int)GetValue(DataCountProperty); }
            set { 
                SetValue(DataCountProperty, value);
                model.TotalDataCount = value;
                if(value > 0)
                {
                    List<int> l = new List<int>();
                    int v = (int)Math.Ceiling(value * 1.0 / model.currentPageSize);
                    for (int i = 1; i <= v; i++)
                    {
                        l.Add(i);
                    }

                    if (model.CurrentPageIndex == 0)
                        model.CurrentPageIndex = 1;
                    model.TotalPageSize = v;
                    model.JumpPages = l;
                }
                else
                {
                    model.CurrentPageIndex = 0;
                    model.TotalPageSize = 0;
                    model.JumpPages = new List<int> { 0 };
                }
                
            }
        }

        private void FontPage_Click(object sender, RoutedEventArgs e)
        {
            if(model.CurrentPageIndex != 1)
            {
                model.CurrentPageIndex = 1;
                RefreshData(model.CurrentPageIndex, model.currentPageSize);
            }
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if(model.CurrentPageIndex > 1)
            {
                model.CurrentPageIndex = model.CurrentPageIndex - 1;
                RefreshData(model.CurrentPageIndex, model.currentPageSize);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if(model.CurrentPageIndex < model.TotalPageSize)
            {
                model.CurrentPageIndex = model.CurrentPageIndex + 1;
                RefreshData(model.CurrentPageIndex, model.currentPageSize);
            }
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {

            if (model.CurrentPageIndex != model.TotalPageSize)
            {
                model.CurrentPageIndex = model.TotalPageSize;
                RefreshData(model.CurrentPageIndex, model.currentPageSize);
            }
        }

        private void JumpPage_Click(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedValue == null) return;
            model.CurrentPageIndex = (int)cb.SelectedValue;
            RefreshData(model.CurrentPageIndex, model.currentPageSize);
        }

        private void PageSize_Click(object sender, SelectionChangedEventArgs e)
        {
            JumpPagesCB.SelectedIndex = 0;
            ComboBox cb = sender as ComboBox;
            model.currentPageSize = (int)cb.SelectedValue;
            model.CurrentPageIndex = 1;
            RefreshData(1, model.currentPageSize);
        }

        private void RefreshData(int currentPageIndex, int currentPageSize)
        {
            PageChanged(currentPageIndex, currentPageSize);
        }
    }
}
