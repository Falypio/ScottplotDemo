using Prism.DryIoc;
using Prism.Ioc;
using PrismAppDemo.Common;
using PrismAppDemo.ViewModels.PageView;
using PrismAppDemo.Views;
using PrismAppDemo.Views.PageView;
using System.Windows;

namespace PrismAppDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            containerRegistry.GetContainer();
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry.RegisterForNavigation<ViewB, ViewBViewModel>();
            containerRegistry.RegisterForNavigation<ViewC, ViewCViewModel>();
            containerRegistry.RegisterForNavigation<ViewD, ViewDViewModel>();
            containerRegistry.RegisterForNavigation<ViewE, ViewEViewModel>();
        }
    }
}
