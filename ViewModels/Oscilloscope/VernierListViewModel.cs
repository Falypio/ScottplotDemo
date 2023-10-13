using Prism.Commands;
using Prism.Mvvm;
using PrismAppDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PrismAppDemo.ViewModels.Oscilloscope
{
    public class VernierListViewModel : BindableBase
    {
        private IListViewItemListener iListener = null!;

        private ObservableCollection<VernierModel> vernierModels;
        public ObservableCollection<VernierModel> VernierModels
        {
            get { return vernierModels; }
            set { SetProperty(ref vernierModels, value); }
        }

        private VernierModel selectedVernier;
        public VernierModel SelectedVernier
        {
            get { return selectedVernier; }
            set { SetProperty(ref selectedVernier, value); }
        }

        public VernierListViewModel(IListViewItemListener lisenter)
        {
            iListener = lisenter;
        }

        private ICommand channelAddCommand;
        public ICommand ChannelAddCommand => channelAddCommand ??= new DelegateCommand(ExecuteChannelAddCommand);

        private void ExecuteChannelAddCommand()
        {
            VernierModel channel = new VernierModel(this.GetHashCode(), $"标尺{this.GetHashCode()}");
            //channel.ChannelUid = this.GetHashCode()
            VernierModels.Add(channel);
            iListener?.OnAdded(channel.VernierUid, channel.VernierName);
        }

        private ICommand channelDelCommand;
        public ICommand ChannelDelCommand => channelDelCommand ??= new DelegateCommand<object>(ExecuteChannelDelCommand);

        private void ExecuteChannelDelCommand(object id)
        {
            SelectedVernier = VernierModels.Where(x => x.VernierUid.Equals(id)).FirstOrDefault();
            iListener?.OnRemoved(SelectedVernier.VernierUid);

            VernierModels.Remove(SelectedVernier);
        }
    }

    public class VernierModel : BindableBase
    {
        private int vernierUid;
        public int VernierUid
        {
            get { return vernierUid; }
            set { SetProperty(ref vernierUid, value); }
        }

        private string vernierName;
        public string VernierName
        {
            get { return vernierName; }
            set { SetProperty(ref vernierName, value); }
        }

        public VernierModel()
        {

        }

        public VernierModel(int vernierUid, string vernierName)
        {
            this.vernierUid = vernierUid;
            this.vernierName = vernierName;
        }
    }
}
