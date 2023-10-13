using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismAppDemo.ViewModels.PageView
{
    public class ViewAViewModel : BindableBase
    {
        private Dictionary<string, object> m_dataList;
        public Dictionary<string, object> DataList
        {
            get { return m_dataList; }
            set { SetProperty(ref m_dataList, value); }
        }

        private ObservableCollection<string> m_demolist;

        public ObservableCollection<string> Demolist
        {
            get { return m_demolist; }
            set { SetProperty(ref m_demolist, value); }
        }

        private Dictionary<string, object> m_selectedItems;
        public Dictionary<string, object> SelectedItems
        {
            get { return m_selectedItems; }
            set { SetProperty(ref m_selectedItems, value); }
        }
        public ViewAViewModel()
        {
            CommandAtt = new DelegateCommand(ExecuteCommandAtt);
            Demolist = new ObservableCollection<string>();
            m_selectedItems = new Dictionary<string, object>();
            m_dataList = new Dictionary<string, object>();
            for (int i = 0; i < 10; i++)
            {
                DataList.Add(i.ToString(), "00" + i);
                Demolist.Add($"cctv{i.ToString()}");
            }
        }
        public DelegateCommand CommandAtt { get; set; }

        public void ExecuteCommandAtt()
        {
            var s = m_selectedItems;
        }
    }
}
