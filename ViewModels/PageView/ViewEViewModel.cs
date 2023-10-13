using AIStudio.Wpf.DiagramDesigner;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PrismAppDemo.ViewModels.PageView
{
    public class ViewEViewModel : BaseViewModel
    {
        public ViewEViewModel()
        {
            Title = "Zoom";
            Info = "Drag the upper-right scroll bar to make the canvas larger and smaller.";

            Diagram = new DiagramViewModel();
            Diagram.DiagramOption.LayoutOption.PageSizeType = PageSizeType.Custom;
            Diagram.DiagramOption.LayoutOption.PageSize = new Size(double.NaN, double.NaN);
            Diagram.ColorViewModel = new ColorViewModel();
            Diagram.ColorViewModel.FillColor.Color = System.Windows.Media.Colors.Orange;

            DefaultDesignerItemViewModel node1 = new DefaultDesignerItemViewModel(Diagram) { Left = 50, Top = 50, Text = "1" };
            Diagram.Add(node1);

            DefaultDesignerItemViewModel node2 = new DefaultDesignerItemViewModel(Diagram) { Left = 300, Top = 300, Text = "2" };
            Diagram.Add(node2);

            DefaultDesignerItemViewModel node3 = new DefaultDesignerItemViewModel(Diagram) { Left = 300, Top = 50, Text = "3" };
            Diagram.Add(node3);

            ConnectionViewModel connector1 = new ConnectionViewModel(Diagram, node1.RightConnector, node2.LeftConnector, DrawMode.ConnectingLineSmooth, RouterMode.RouterNormal);
            connector1.ColorViewModel.LineDashStyle = LineDashStyle.Dash1;
            connector1.AnimationViewModel.Animation = LineAnimation.DashAnimation;
            Diagram.Add(connector1);

            ConnectionViewModel connector2 = new ConnectionViewModel(Diagram, node2.RightConnector, node3.RightConnector, DrawMode.ConnectingLineStraight, RouterMode.RouterOrthogonal);
            connector2.ColorViewModel.LineDashStyle = LineDashStyle.Dash1;
            connector2.AnimationViewModel.Animation = LineAnimation.DashAnimation;
            Diagram.Add(connector2);
        }
    }
    public class BaseViewModel : Prism.Mvvm.BindableBase
    {

        private DiagramViewModel diagramViewModel;
        public DiagramViewModel Diagram
        {
            get { return diagramViewModel; }
            set { SetProperty(ref diagramViewModel, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        private string info;
        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value); }
        }
    }
}
