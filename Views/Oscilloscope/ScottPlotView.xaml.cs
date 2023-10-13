using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using AIStudio.Wpf.DiagramDesigner;
using ImTools;
using ScottPlot;
using ScottPlot.Plottable;

namespace PrismAppDemo.Views.Oscilloscope
{
    /// <summary>
    /// Interaction logic for ScottPlotView
    /// </summary>
    public partial class ScottPlotView : UserControl
    {
        #region 私有变量

        /// <summary>
        /// 每段信号最大容量
        /// </summary>
        private static int pointMaxCount = 100000;

        /// <summary>
        /// 选中高亮坐标点
        /// </summary>
        private readonly MarkerPlot HighlightedPoint;

        /// <summary>
        /// 通道
        /// </summary>
        private Dictionary<string, SignalPlot> renderInfos = new Dictionary<string, SignalPlot>();

        /// <summary>
        /// 标尺
        /// </summary>
        private Dictionary<string, AxisLine> axisLineMap = new Dictionary<string, AxisLine>();

        /// <summary>
        /// 高亮波形key
        /// </summary>
        private string lastHightlight = string.Empty;

        private Random rd = new Random();

        #endregion


        public readonly DispatcherTimer reanderTimer;

        /// <summary>
        /// 采样频率
        /// </summary>
        public double SampleRate = 2000.0;

        public List<double> ZoomScopeList = new List<double>();
        public ScottPlotView()
        {
            InitializeComponent();

            HighlightedPoint = CreateHighlightedPoint();
            InitScattPlot(component);
            ZoomScopeArray();

            reanderTimer = new DispatcherTimer();

            reanderTimer.Interval = TimeSpan.FromMilliseconds(10);

            reanderTimer.Tick += OnRender;
        }

        private void InitScattPlot(WpfPlot component)
        {
            component.Plot.BottomAxis.Ticks(true);//false隐藏底X轴 true 显示
            component.Plot.SetAxisLimits(0, 2000, -100, 100);//自定义初始Y轴与X轴范围
            //component.Plot.YAxis.SetZoomInLimit(100);//Y最小范围
            //component.Plot.YAxis.SetZoomOutLimit(1000);//Y最大范围
            //component.Plot.XAxis.SetZoomInLimit(10);//X最小范围
             //component.Plot.XAxis.SetZoomOutLimit(pointMaxCount);//X最大范围
            //component.Plot.TopAxis.Ticks(true);//显示顶X轴
            //component.Plot.TopAxis.SetZoomInLimit(100);//顶X最小范围
            //component.Plot.TopAxis.SetZoomOutLimit(1000);//顶X最大范围
            //component.Plot.TopAxis.SetSizeLimit(0, 1000);
            //component.Configuration.RightClickDragZoom = false;//拖动缩放
            component.PreviewMouseWheel += Component_PreviewMouseWheel;
            component.MouseLeftButtonDown += WpfPlot_MouseDown;
            component.Plot.Style(figureBackground: Color.White, dataBackground: Color.White, Color.LightGray);//示波器样式
            InstallGridding();
            component.Refresh();
        }

        private void Component_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ZoomScopeArray();
        }

        #region 示波器设置



        private void ZoomScopeArray()
        {
            //double sad = SampleRate;
            //while (sad>10)
            //{
            //    ZoomScopeList.Add(sad);
            //    sad /= 2;  
            //}

            // 获取当前X轴的范围
            double xMin = (int)component.Plot.GetAxisLimits().XMin;
            double xMax = (int)component.Plot.GetAxisLimits().XMax;

            // 根据X轴范围计算刻度间隔
            double xRange = xMax - xMin;
            double tickSpacing = xRange / 10; // 这里设置为10个刻度

            // 设置X轴的刻度间隔
            component.Plot.XAxis.ManualTickSpacing(tickSpacing, ScottPlot.Ticks.DateTimeUnit.Decisecond);
            //component.Plot.XAxis.MinimumTickSpacing(tickSpacing);

            component.Plot.SetAxisLimitsX(xMin, xMax);
            // 更新绘图
            component.Refresh();
        }

        private void InstallGridding()
        {
            //component.Plot.XAxis.AxisTicks.MajorLineWidth = 5;
            //component.Plot.XAxis.AxisTicks.MinorLineWidth = 2;
            //// 设置X轴的刻度间隔
            //component.Plot.XAxis.TickDensity(1);
            //component.Plot.XAxis.MinimumTickSpacing(100);//刻度范围
            //component.Plot.XAxis.MinorLogScale(enable:true, roundMajorTicks:true,1);
            //component.Plot.YAxis.SetBoundary(-200, 200);
            component.Plot.YAxis.SetBoundary(-500, 500);
            component.Plot.XAxis.SetBoundary(0, pointMaxCount);
            //component.Plot.XAxis.MinimumTickSpacing(10);

            //component.Plot.XAxis.SetSizeLimit(0, 10);

            component.Configuration.ScrollWheelZoomFraction = 0.2;
        }

        /// <summary>
        /// 计时开始
        /// </summary>
        public void TimerStart()
        {
            if (renderInfos.Count == 0) return;
            component.Plot.SetAxisLimitsX(pointMaxCount - 1000, pointMaxCount);
            reanderTimer.Start();
        }


        /// <summary>
        /// 停止计时
        /// </summary>
        public void TimerStop()
        {
            reanderTimer.Stop();
        }

        private void OnRender(object? sender, EventArgs e)
        {
            RefreshOPMUI();
            component.Refresh();
        }

        private MarkerPlot CreateHighlightedPoint()
        {

            MarkerPlot markerPlot = component.Plot.AddPoint(0, 0, color: Color.Red, size: 10, shape: MarkerShape.openCircle);
            markerPlot.IsVisible = false;
            markerPlot.TextFont.Alignment = Alignment.LowerCenter;
            markerPlot.TextFont.Size = 16;
            markerPlot.TextFont.Bold = true;

            return markerPlot;
        }

        /// <summary>
        /// 左键绘图区域显示当前选中通道的数据点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpfPlot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (renderInfos != null)
            {
                (double x, double y) = GetMouseNearestPoint();
                HighlightedPoint.X = x;
                HighlightedPoint.Y = y;
                HighlightedPoint.IsVisible = true;
                HighlightedPoint.Text = string.Format("{0}", y);
            }
        }



        /// <summary>
        /// 获取距离鼠标最近的点
        /// </summary>
        /// <returns></returns>
        private (double x, double y) GetMouseNearestPoint()
        {

            double nearestY = 0;

            double nearestX = 0;

            (double mouseCoordX, double mouseCoordY) = component.GetMouseCoordinates();

            var results = renderInfos.Values.Select(x => x.GetPointNearestX(mouseCoordX));

            if (results != null && results.Count() > 0)
            {

                double num = double.MaxValue;

                foreach (var result in results)
                {
                    if (Math.Abs(result.y - mouseCoordY) < num)
                    {
                        nearestY = result.y;
                        nearestX = result.x;
                    }
                }
            }
            return (nearestX, nearestY);
        }


        /// <summary>
        /// 模拟参数
        /// </summary>
        public async void RefreshOPMUI()
        {
            foreach (var result in renderInfos.Values)
            {
                int v1 = rd.Next(0, 100);
                Array.Copy(result.Ys, 1, result.Ys, 0, result.Ys.Length - 1);
                result.Ys[^1] = v1;
            }
        }


        #endregion

        #region 通道

        /// <summary>
        /// 添加通道
        /// </summary>
        internal bool GenerateSignal(string key,string name)
        {

            //创建波形图实例        
            double[] ys = new double[pointMaxCount];

            RemoveSignal(key);

            //string name = string.Format("通道{0}", key);

            SignalPlot plot = component.Plot.AddSignal(ys, 1, label: name);
            //component.Plot.AxisAutoX(margin: 0);

            return renderInfos.TryAdd(key, plot);
        }


        /// <summary>
        /// 选择通道
        /// </summary>
        /// <param name="key"></param>
        internal void ChannelSelected(string key)
        {
            if (lastHightlight.Equals(key) || !renderInfos.ContainsKey(key)) return;

            if (!string.IsNullOrEmpty(lastHightlight))
            {
                renderInfos[lastHightlight].MarkerSize = 3;
                renderInfos[lastHightlight].LineWidth = 1;
            }

            renderInfos[key].MarkerSize = 8;
            renderInfos[key].LineWidth = 3;
            lastHightlight = key;
        }

        /// <summary>
        /// 移除通道
        /// </summary>
        /// <param name="key"></param>
        internal void RemoveSignal(string key)
        {
            if (renderInfos.ContainsKey(key))
            {
                var info = renderInfos[key];

                // if (renderInfos.TryRemove(key, out var target))
                {
                    component.Plot.Remove(info);
                }
                renderInfos.Remove(key);
            }
        }

        /// <summary>
        /// 清空通道
        /// </summary>
        internal void ClearSignals()
        {
            ClearAxisLine();
            foreach (var item in renderInfos)
            {
                component.Plot.Remove(item.Value);
            }
            renderInfos?.Clear();
        }

        #endregion

        #region 标尺

        /// <summary>
        /// 清空标尺
        /// </summary>
        internal void ClearAxisLine()
        {
            foreach (var item in axisLineMap)
            {
                component.Plot.Remove(item.Value);
            }
            axisLineMap?.Clear();
        }

        #endregion
    }
}
