using AIStudio.Wpf.DiagramDesigner.Geometrys;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace PrismAppDemo.Common.Themes
{
    public class FlowLine : System.Windows.Shapes.Shape
    {
        private LineGeometry FlowLineGeometry { get; set; }

        private  DoubleCollection FlowDoubleCollection { get; set; }

        private  DoubleAnimation FlowDoubleAnimation { get; set; }

        private  Storyboard FlowStoryboard { get; set; }


        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register(nameof(X1), typeof(double), typeof(FlowLine), new PropertyMetadata(5.0,new PropertyChangedCallback(OnGeometryChanged)));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register(nameof(Y1), typeof(double), typeof(FlowLine), new PropertyMetadata(5.0, new PropertyChangedCallback(OnGeometryChanged)));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register(nameof(X2), typeof(double), typeof(FlowLine), new PropertyMetadata(5.0, new PropertyChangedCallback(OnGeometryChanged)));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register(nameof(Y2), typeof(double), typeof(FlowLine), new PropertyMetadata(5.0, new PropertyChangedCallback(OnGeometryChanged)));

        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowLine flowLine)
            {
                flowLine.FlowLineGeometry = new LineGeometry(new Point(flowLine.X1, flowLine.Y1), new Point(flowLine.X2, flowLine.Y2));
            }
        }

        public static readonly DependencyProperty IsDashedProperty =
            DependencyProperty.Register(nameof(IsDashed), typeof(bool), typeof(FlowLine), new PropertyMetadata(false, OnIsDashedChanged));
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
            FlowLine FlowLine = (FlowLine)d;
            FlowLine.FlowDoubleCollection.Clear();
            if (FlowLine.IsDashed)
            {
                FlowLine.FlowDoubleCollection.Add(2); // 实线长度
                FlowLine.FlowDoubleCollection.Add(2); // 虚线长度
            }
            else
            {
                FlowLine.FlowDoubleCollection.Add(2); // 实线长度
                FlowLine.FlowDoubleCollection.Add(0); // 虚线长度
            }
            // 设置虚线模式到线条的StrokeDashArray属性
            FlowLine.StrokeDashArray = FlowLine.FlowDoubleCollection;
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
            DependencyProperty.Register(nameof(IsFlow), typeof(FlowDirection), typeof(FlowLine), new PropertyMetadata(FlowDirection.Stop, OnIsFlowChanged));

        private static void OnIsFlowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowLine flowLine = (FlowLine)d;
            if (flowLine.IsFlow != FlowDirection.Stop)
            {
                GetFlowStoryboard(flowLine);
            }
            else
            {
                flowLine.FlowStoryboard.Stop();
            }
        }

        private static void GetFlowStoryboard(FlowLine flowLine)
        {
            flowLine.FlowStoryboard.Children.Clear();
            flowLine.FlowStoryboard.RepeatBehavior = RepeatBehavior.Forever;//动画重复执行方式
            flowLine.FlowDoubleAnimation.Duration = TimeSpan.FromSeconds(2);//动画执行时间
            if (flowLine.IsFlow == FlowDirection.Left)
            {
                flowLine.FlowDoubleAnimation.From = 20;
                flowLine.FlowDoubleAnimation.To = 0;
            }
            else
            {
                flowLine.FlowDoubleAnimation.From = 0;
                flowLine.FlowDoubleAnimation.To = 20;
            }
            flowLine.FlowDoubleAnimation.AutoReverse = false;
            Storyboard.SetTarget(flowLine.FlowDoubleAnimation, flowLine);
            Storyboard.SetTargetProperty(flowLine.FlowDoubleAnimation, new PropertyPath("StrokeDashOffset"));//动画主要控制虚线的偏移量来实现流动效果
            flowLine.FlowStoryboard.Children.Add(flowLine.FlowDoubleAnimation);
            flowLine.FlowStoryboard.Begin();
        }

        public FlowLine()
        {
            FlowLineGeometry = new LineGeometry(new Point(this.X1, this.Y1), new Point(this.X2, this.Y2));
            FlowDoubleCollection = new DoubleCollection();
            FlowDoubleAnimation = new DoubleAnimation();
            FlowStoryboard = new Storyboard();
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                return FlowLineGeometry;
            }
        }
        public enum FlowDirection
        {
            Left,
            Right,
            Stop
        }

    }
}
