using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismAppDemo.Common.EventMessages;
using PrismAppDemo.Interfaces;
using PrismAppDemo.ViewModels.Oscilloscope;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismAppDemo.ViewModels.PageView
{
    public class ViewDViewModel : BindableBase
    {
        private readonly IDialogService m_dialogService;

        private readonly IEventAggregator m_aggregator;

        public ChannelListViewModel ChannelListModel { get; set; }

        public VernierListViewModel VernierListModel { get; set; }
        public ViewDViewModel(IEventAggregator eventAggregator)
        {
            this.m_aggregator = eventAggregator;

            ChannelListModel = new ChannelListViewModel(new ChannelListViewItemListener(m_aggregator));

            VernierListModel = new VernierListViewModel(new VernierListViewItemListener(m_aggregator));
        }
    }

    internal class ChannelListViewItemListener : IListViewItemListener
    {
        private readonly IEventAggregator eventAggregator;

        public ChannelListViewItemListener(IEventAggregator aggregator)
        {
            eventAggregator = aggregator;
        }

        public void OnAdded(int key, string name)
        {
            eventAggregator.GetEvent<AddChanelEvent>().Publish(new ItemAddedRecord(key, name));
        }

        public void OnCleared()
        {
            throw new NotImplementedException();
        }

        public void OnRemoved(int key)
        {
            eventAggregator.GetEvent<DelChanelEvent>().Publish(key);
        }

        public void OnSelectChanged(int key)
        {
            eventAggregator.GetEvent<SelectChannelEvent>().Publish(key);
        }

        public void OnVisibilityChanged(bool visbility, int key)
        {
            throw new NotImplementedException();
        }
    }

    internal class VernierListViewItemListener : IListViewItemListener
    {
        private readonly IEventAggregator eventAggregator;

        public VernierListViewItemListener(IEventAggregator aggregator)
        {
            eventAggregator = aggregator;
        }
        public void OnAdded(int key, string name)
        {
            eventAggregator.GetEvent<AddVernierEvent>().Publish(new ItemAddedRecord(key, name));
        }

        public void OnCleared()
        {
            throw new NotImplementedException();
        }

        public void OnRemoved(int key)
        {
            eventAggregator.GetEvent<DelVernierEvent>().Publish(key);
        }

        public void OnSelectChanged(int key)
        {
            return;
        }

        public void OnVisibilityChanged(bool visbility, int key)
        {
            throw new NotImplementedException();
        }
    }
}
