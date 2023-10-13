using ControlzEx.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrismAppDemo.Common.Themes
{
    /// <summary>
    /// FlowLineControl.xaml 的交互逻辑
    /// </summary>
    public partial class FlowLineControl : UserControl
    {
        private DoubleCollection FlowDoubleCollection { get; set; }

        private DoubleAnimation FlowDoubleAnimation { get; set; }

        private Storyboard FlowStoryboard { get; set; }


        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register(nameof(X1), typeof(double), typeof(FlowLineControl), new PropertyMetadata(0.0));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register(nameof(Y1), typeof(double), typeof(FlowLineControl), new PropertyMetadata(0.0));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register(nameof(X2), typeof(double), typeof(FlowLineControl), new PropertyMetadata(0.0));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register(nameof(Y2), typeof(double), typeof(FlowLineControl), new PropertyMetadata(0.0));

        
        /// <summary>
        /// Stroke
        /// </summary>
        public Brush FlowBrush
        {
            get { return (Brush)GetValue(FlowBrushProperty); }
            set { SetValue(FlowBrushProperty, value); }
        }

        public static readonly DependencyProperty FlowBrushProperty =
            DependencyProperty.Register(nameof(FlowBrush), typeof(Brush), typeof(FlowLineControl), new PropertyMetadata(Brushes.Black));


        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(int), typeof(FlowLineControl), new PropertyMetadata(1));




        public static readonly DependencyProperty IsDashedProperty =
            DependencyProperty.Register(nameof(IsDashed), typeof(bool), typeof(FlowLineControl), new PropertyMetadata(false, OnIsDashedChanged));

        /// <summary>
        /// 是否虚线
        /// </summary>
        public bool IsDashed
        {
            get { return (bool)GetValue(IsDashedProperty); }
            set { SetValue(IsDashedProperty, value); }
        }

        private static void OnIsDashedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowLineControl flow)
            {
                flow.FlowDoubleCollection.Clear();
                if (flow.IsDashed)
                {
                    flow.FlowDoubleCollection.Add(2); // 实线长度
                    flow.FlowDoubleCollection.Add(2); // 虚线长度
                }
                else
                {
                    flow.FlowDoubleCollection.Add(2); // 实线长度
                    flow.FlowDoubleCollection.Add(0); // 虚线长度
                }
                // 设置虚线模式到线条的StrokeDashArray属性
                flow.flowLine.StrokeDashArray = flow.FlowDoubleCollection;
            }

        }
        /// <summary>
        /// 流动效果
        /// </summary>
        public FlowDirection IsFlow
        {
            get { return (FlowDirection)GetValue(IsFlowProperty); }
            set { SetValue(IsFlowProperty, value); }
        }

        public static readonly DependencyProperty IsFlowProperty =
            DependencyProperty.Register(nameof(IsFlow), typeof(FlowDirection), typeof(FlowLineControl), new PropertyMetadata(FlowDirection.Stop, OnIsFlowChanged));

        private static void OnIsFlowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowLineControl flow)
            {
                if (flow.IsFlow != FlowDirection.Stop)
                {
                    GetFlowStoryboard(flow);
                }
                else
                {
                    flow.FlowStoryboard.Stop();
                }
            }

        }

        private static void GetFlowStoryboard(FlowLineControl flow)
        {
            flow.FlowStoryboard.Children.Clear();
            flow.FlowStoryboard.RepeatBehavior = RepeatBehavior.Forever;//动画重复执行方式
            flow.FlowDoubleAnimation.Duration = TimeSpan.FromSeconds(5);//动画执行时间
            if (flow.IsFlow == FlowDirection.Left)
            {
                flow.FlowDoubleAnimation.From = 20;
                flow.FlowDoubleAnimation.To = 0;
            }
            else
            {
                flow.FlowDoubleAnimation.From = 0;
                flow.FlowDoubleAnimation.To = 20;
            }
            flow.FlowDoubleAnimation.AutoReverse = false;
            Storyboard.SetTarget(flow.FlowDoubleAnimation, flow.flowLine);
            Storyboard.SetTargetProperty(flow.FlowDoubleAnimation, new PropertyPath("StrokeDashOffset"));//动画主要控制虚线的偏移量来实现流动效果
            flow.FlowStoryboard.Children.Add(flow.FlowDoubleAnimation);
            flow.FlowStoryboard.Begin();
        }
        public FlowLineControl()
        {
            InitializeComponent();
            FlowDoubleCollection = new DoubleCollection();
            FlowDoubleAnimation = new DoubleAnimation();
            FlowStoryboard = new Storyboard();
        }

        public enum FlowDirection
        {
            Left,
            Right,
            Stop
        }
    }

}
