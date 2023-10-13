using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using PrismAppDemo.Views.PageView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrismAppDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventAggregator aggregator;

        private readonly IContainerProvider provider;

        private readonly IRegionManager regionManager;

        public MainWindow(IEventAggregator eventAggregator, IContainerProvider container, IRegionManager regionManager)
        {
            this.aggregator = eventAggregator;

            this.provider = container;

            this.regionManager = regionManager;

            InitializeComponent();

            regionManager.RegisterViewWithRegion<ViewA>("ViewA");
            regionManager.RegisterViewWithRegion<ViewB>("ViewB");
            regionManager.RegisterViewWithRegion<ViewC>("ViewC");
            regionManager.RegisterViewWithRegion<ViewD>("ViewD");
            regionManager.RegisterViewWithRegion<ViewE>("ViewE");
        }


        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            //判断是否以及最大化，最大化就还原窗口，否则最大化
            if (this.WindowState == WindowState.Maximized)
            { 
                this.WindowState = WindowState.Normal;
                txtMax.Text = $"\ue618";
            }
            else
            { 
                this.WindowState = WindowState.Maximized;
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                WindowState = WindowState.Maximized;
                txtMax.Text = $"\ue61c";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
