using Prism.Events;
using PrismAppDemo.Common.EventMessages;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PrismAppDemo.Views.PageView
{
    /// <summary>
    /// Interaction logic for ViewD
    /// </summary>
    public partial class ViewD : UserControl
    {

        private readonly IEventAggregator aggregator;
        public ViewD(IEventAggregator eventAggregator)
        {
            this.aggregator = eventAggregator;

            EventAggregatorSubscribe(this.aggregator);
            InitializeComponent();
        }

        private void EventAggregatorSubscribe(IEventAggregator aggregator)
        {
            aggregator.GetEvent<AddChanelEvent>().Subscribe(OnChannelAddedCallback);
            aggregator.GetEvent<DelChanelEvent>().Subscribe(OnChannelDeledCallback);
            aggregator.GetEvent<SelectChannelEvent>().Subscribe(OnChannelSelectedCallback);

        }

        private void OnChannelAddedCallback(ItemAddedRecord record)
        {
            Plot.GenerateSignal(record.id.ToString(), record.name);
        }

        private void OnChannelDeledCallback(int id)
        {
            Plot.RemoveSignal(id.ToString());
        }
        private void OnChannelSelectedCallback(int id)
        {
            Plot.ChannelSelected(id.ToString());
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Plot.TimerStart();
        }

        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            Plot.TimerStop();
        }
    }
}
