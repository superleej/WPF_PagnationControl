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
    public class PaginationModel : INotifyPropertyChanged
    {


        private int _SelectedIndex;

        public PaginationModel()
        {
            FirstPage = new RelayCommand(CMD_FirstPage);
            PrevPage = new RelayCommand(CMD_PrevPage);
            NextPage = new RelayCommand(CMD_NextPage);
            LastPage = new RelayCommand(CMD_LastPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Current { get; set; }

        public RelayCommand FirstPage { get; }

        public List<object> Items { get; set; }

        public RelayCommand LastPage { get; }

        public RelayCommand NextPage { get; }

        public RelayCommand PrevPage { get; }

        private List<object> dataList;

        public SortedDictionary<int, List<object>> pagedList;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex == value)
                    return;
                _SelectedIndex = value;
                if (_SelectedIndex < 0)
                    return;
                Current = _SelectedIndex + 1;
                RefreashPage(Current);
            }
        }

        public List<string> SelectList { get; set; }

        public int Total { get; set; }

        public void FreashData(List<object> list)
        {
            dataList = list;
            pagedList = new SortedDictionary<int, List<object>>();
            for (int i = 0; i < list.Count; i += CurrentSplit)
            {
                pagedList[i / CurrentSplit] = list.Skip(i).Take(CurrentSplit).ToList();
            }
            Current = 1;
            Total = pagedList.Count;
            RefreashPage(Current);
            SelectList = pagedList.ToList().ConvertAll(o => $"第{o.Key + 1}页");
            RaisProperyChanged("SelectList");
        }

        private void CMD_FirstPage(object obj)
        {
            if (pagedList == null || !pagedList.Any())
                return;
            if (Current <= 1)
                return;
            Current = 1;
            RefreashPage(Current);
        }

        private void CMD_LastPage(object obj)
        {
            if (pagedList == null || !pagedList.Any())
                return;
            if (Current >= Total)
                return;
            Current = Total;
            RefreashPage(Current);
        }

        private void CMD_NextPage(object obj)
        {
            if (pagedList == null || !pagedList.Any())
                return;
            if (Current >= Total)
                return;
            Current++;
            RefreashPage(Current);
        }

        private void CMD_PrevPage(object obj)
        {
            if (pagedList == null || !pagedList.Any())
                return;
            if (Current <= 1)
                return;
            Current--;
            RefreashPage(Current);
        }

        void RaisProperyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        void RefreashPage(int index)
        {
            Items = pagedList[index - 1];
            RaisProperyChanged("Items");
            RaisProperyChanged("Current");
            RaisProperyChanged("Total");
        }
        public List<int> SplitCount { get; set; } = new List<int>() { 5, 10, 20, 50, 100, 200, 500, 1000 };
        private int _CurrentSplit = 100;

        public int CurrentSplit
        {
            get { return _CurrentSplit; }
            set
            {
                if (_CurrentSplit == value)
                    return;
                _CurrentSplit = value;
                if (dataList != null)
                    FreashData(dataList);
            }
        }
    }




    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 判断命令是否可以执行的方法
        /// </summary>
        private Func<object, bool> _canExecute;

        /// <summary>
        /// 命令需要执行的方法
        /// </summary>
        private Action<object> _execute;

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="execute">命令要执行的方法</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="execute">命令要执行的方法</param>
        /// <param name="canExecute">判断命令是否能够执行的方法</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 检查命令是否可以执行的事件，在UI事件发生导致控件状态或数据发生变化时触发
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        /// <summary>
        /// 判断命令是否可以执行
        /// </summary>
        /// <param name="parameter">命令传入的参数</param>
        /// <returns>是否可以执行</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute(parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (_execute != null && CanExecute(parameter))
            {
                _execute(parameter);
            }
        }

    }
}
