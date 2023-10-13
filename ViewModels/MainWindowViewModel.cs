using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using PrismAppDemo.Common;
using PrismAppDemo.Extensions;
using PrismAppDemo.Models;
using PrismAppDemo.ViewModels.PageView;
using PrismAppDemo.Views.PageView;
using ScottPlot.Styles;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace PrismAppDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<MenuCless> m_menuClessList;
        public ObservableCollection<MenuCless> MenuClessList
        {
            get { return m_menuClessList; }
            set { SetProperty(ref m_menuClessList, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            MenuClessList = new ObservableCollection<MenuCless>();
            NavigateCommand = new DelegateCommand<MenuCless>(Navigate);
            //NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();//导航到最近一条历史页
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();//导航到正向的页
            });
            this.regionManager = regionManager;
            //for (int i = 0; i < 6; i++)
            //{
            //    MenuCless menuCless = new MenuCless();
            //    menuCless.MenuName = $"菜单{i}";
            //    menuCless.Icon = "ChangeOverImage";
            //    menuCless.MenuColor = Brushes.White;
            //    MenuClessList.Add(menuCless);
            //}
        }
        public DelegateCommand<MenuCless> NavigateCommand { get; private set; }
        //public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

        private void Navigate(MenuCless obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        private void Navigate(string obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj))
                return;

            //regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj, back =>
            //{
            //    journal = back.Context.NavigationService.Journal;
            //});
            regionManager.RequestNavigate(obj, obj);
        }

        void CreateMenuBar()
        {
            MenuClessList.Add(new MenuCless() { MenuName="基尼泰库拉", MenuColor = Brushes.Transparent, Icon = "Scope", NameSpace = "ViewA" });
            MenuClessList.Add(new MenuCless() { MenuName = "这恒河里", MenuColor = Brushes.White, Icon = "SystemInfo", NameSpace = "ViewB" });
            MenuClessList.Add(new MenuCless() { MenuName = "这泰河里", MenuColor = Brushes.White, Icon = "ExportAllButton", NameSpace = "ViewC" });
            MenuClessList.Add(new MenuCless() { MenuName = "示波器", MenuColor = Brushes.White, Icon = "QueryButton", NameSpace = "ViewD" });
            MenuClessList.Add(new MenuCless() { MenuName = "FBI", MenuColor = Brushes.White, Icon = "Folder", NameSpace = "ViewE" });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("ViewA");

            //regionManager.RegisterViewWithRegion<ViewA>("ViewA");
            //regionManager.RegisterViewWithRegion<ViewB>("ViewB");
            //regionManager.RegisterViewWithRegion<ViewC>("ViewC");
            //regionManager.RegisterViewWithRegion<ViewD>("ViewD");
            //regionManager.RegisterViewWithRegion<ViewE>("ViewE");
        }
    }
}
