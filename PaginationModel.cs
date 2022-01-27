using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Input;

namespace DataAnalysisWpf.Controls
{
    public class Pagination1Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int currentPageIndex;
        public int CurrentPageIndex { get { return currentPageIndex; } set { currentPageIndex = value; OnPropertyChanged("CurrentPageIndex"); } }

        public int currentPageSize { get; set; }
        public int CurrentPageSize { get { return currentPageSize; } set { currentPageSize = value; OnPropertyChanged("CurrentPageSize"); } }
        public int totalPageSize { get; set; }
        public int TotalPageSize { get { return totalPageSize; } set { totalPageSize = value; OnPropertyChanged("TotalPageSize"); } }
        public int totalDataCount { get; set; }
        public int TotalDataCount { get { return totalDataCount; } set { totalDataCount = value; OnPropertyChanged("TotalDataCount"); } }

        public List<int> jumpPages { get; set; }
        public List<int> JumpPages { get { return jumpPages; } set { jumpPages = value; OnPropertyChanged("JumpPages"); } }

        public List<int> chosePageSizes { get; set; }
        public List<int> ChosePageSizes { get { return chosePageSizes; } set { chosePageSizes = value; OnPropertyChanged("ChosePageSizes"); } }
    }
}
