using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using PrismAppDemo.Models;
using ScottPlot;
using ScottPlot.Plottable;
using static ScottPlot.Plottable.PopulationPlot;
using System.Xml.Linq;
using ScottPlot.Plottable.AxisManagers;

namespace PrismAppDemo.Views.PageView
{
    /// <summary>
    /// Interaction logic for ViewB
    /// </summary>
    public partial class ViewB : UserControl
    {
        /// <summary>
        /// X最大范围
        /// </summary>
        private int SCOTTPLOTSCOPE = 5000;

        public double[] myData1 = new double[2000];
        public double[] myData2 = new double[2000];
        bool initflag = false;
        int m_countSecond = 0;
        int renderedIndex = 0;
        private ScottPlot.Plottable.SignalPlot signalPlot1;
        private ScottPlot.Plottable.SignalPlot signalPlot2;

        private ScottPlot.Plottable.Crosshair Crosshair;

        public ObservableCollection<AxisLineVernier> AxisLineVerniers;

        private DispatcherTimer m_disTimer = new DispatcherTimer();

        private CancellationTokenSource tokenSource;

        private Task SPdataProcessing;

        private IAxisManager m_axisManager;
        public ViewB()
        {
            InitializeComponent();


            InitDynamicDataDisplay();
            CountDown();
            Crosshair = WpfPlot1.Plot.AddCrosshair(0, 0);
            Crosshair.IsVisible = false;

            AxisLineVerniers = new ObservableCollection<AxisLineVernier>();
            Binding binding = new Binding() { Source = AxisLineVerniers };
            this.LiboxAxis.SetBinding(ListBox.ItemsSourceProperty, binding);

            m_axisManager = new Slide()
            {
                Width = SCOTTPLOTSCOPE,
                PaddingFractionX = 0,
            };
        }

        /// <summary>
        /// 折线图动态数据展示
        /// </summary>
        private void InitDynamicDataDisplay()
        {
            double sampleRate = 24 * 60;
            //myData1 = DataGen.RandomWalk(null, 60 * 8, 10000);
            //myData2 = DataGen.RandomWalk(null, 60 * 8, 10000);
            signalPlot1 = this.WpfPlot1.Plot.AddSignal(myData1, 1, System.Drawing.Color.Black, label: "曲线1");
            signalPlot2 = this.WpfPlot1.Plot.AddSignal(myData2, 1, System.Drawing.Color.Magenta, label: "曲线2");
            //signalPlot1 = this.WpfPlot1.Plot.AddSignal(myData1, sampleRate, System.Drawing.Color.Black);
            //signalPlot2 = this.WpfPlot1.Plot.AddSignal(myData2, sampleRate, System.Drawing.Color.Magenta);


            //WpfPlot1.Plot.XAxis.Label("Time (milliseconds)");


            WpfPlot1.Plot.YAxis.Label("CCTV");
            WpfPlot1.Plot.XAxis2.Label("Title");
            WpfPlot1.Plot.SetAxisLimits(0, 10000, 0, 4000);//自定义Y轴与X轴范围
            WpfPlot1.Plot.SetOuterViewLimits(-500, 10000, -500, 4000);//限制缩放最大范围

            //WpfPlot1.Configuration.LockVerticalAxis = true;//锁定垂直缩放(Y轴)
            //WpfPlot1.Configuration.LockHorizontalAxis = true;//锁定水平缩放(X轴)
            cboxZoom.IsChecked = true;

            // One Axis Only
            //WpfPlot1.Plot.XAxis.Ticks(false);
            //WpfPlot1.Plot.XAxis.Line(false);
            //WpfPlot1.Plot.YAxis2.Line(false);
            //WpfPlot1.Plot.XAxis2.Line(false);

            // 启用X轴时间刻度
            //WpfPlot1.Plot.XAxis.DateTimeFormat(true);
            //WpfPlot1.Plot.XAxis.TickLabelFormat("HH:mm:ss:fff", true);

            // 设置起始时间
            //signalPlot1.OffsetX = DateTime.Now.ToOADate();
            //signalPlot2.OffsetX = DateTime.Now.ToOADate();

            //var culture = System.Globalization.CultureInfo.CreateSpecificCulture("hu"); // Hungarian
            //WpfPlot1.Plot.SetCulture(culture);
            //signalPlot1.MaxRenderIndex = renderedIndex;

            WpfPlot1.Plot.Legend();

            WpfPlot1.Refresh();

            signalPlot1.IsVisible = true;
            signalPlot2.IsVisible = true;

            initflag = true;

            signalPlot1.OffsetX = 0;
            signalPlot2.OffsetX = 0;
        }

        public void CountDown()
        {
            //设置定时器
            m_disTimer.Tick += new EventHandler(DisTimerTick);//每一秒执行的方法
            m_disTimer.Interval = new TimeSpan(10000000); //时间间隔为一秒。

        }
        public void DisTimerTick(object sender, EventArgs e)
        {
            if (m_countSecond == -1)
            {
                m_disTimer.Stop();//计时停止 
                renderedIndex = 0;
                BtnStop.IsEnabled = false;
                BtnStart.IsEnabled = true;
            }
            else
            {
                //判断是否处于UI线程上
                if (txtCountdown.Dispatcher.CheckAccess())
                {
                    txtCountdown.Text = m_countSecond.ToString() + "秒";
                }
                else
                {
                    txtCountdown.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => {
                        txtCountdown.Text = m_countSecond.ToString() + "秒";
                    }));
                }
                m_countSecond--;
            }
        }

        /// <summary>
        /// 数据右滑显示最新数据
        /// </summary>
        /// <param name="force"></param>
        private void UpdateAxisLimits(bool force = false)
        {
            AxisLimits viewLimits = force ? WpfPlot1.Plot.GetAxisLimits(1, 1) : AxisLimits.NoLimits;
            AxisLimits axisLimits = WpfPlot1.Plot.GetDataLimits();
            AxisLimits axisLimits2 = m_axisManager.GetAxisLimits(viewLimits, axisLimits);
            WpfPlot1.Plot.SetAxisLimits(axisLimits2);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxZoom_Checked(object sender, RoutedEventArgs e)
        {
            //鼠标右键拖拽缩放
            this.WpfPlot1.Configuration.RightClickDragZoom = (bool)cboxZoom.IsChecked;

            //鼠标管轮缩放
            this.WpfPlot1.Configuration.ScrollWheelZoom = (bool)cboxZoom.IsChecked;

            WpfPlot1.Configuration.LockVerticalAxis = !(bool)cboxZoom.IsChecked;//锁定X轴缩放
            cboxLockingZoomX.IsEnabled = (bool)cboxZoom.IsChecked;//是否启用X
            WpfPlot1.Configuration.LockVerticalAxis = !(bool)cboxZoom.IsChecked; //锁定Y轴缩放
            cboxLockingZoomY.IsEnabled = (bool)cboxZoom.IsChecked;//是否启用Y
        }

        /// <summary>
        /// 锁定X轴缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxLockingZoomX_Click(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Configuration.LockHorizontalAxis = (bool)cboxLockingZoomX.IsChecked;
        }

        /// <summary>
        ///  锁定Y轴缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxLockingZoomY_Checked(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Configuration.LockVerticalAxis = (bool)cboxLockingZoomY.IsChecked;
        }

        /// <summary>
        /// 鼠标划入十字标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpfPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((bool)cboxLockingCrosshair.IsChecked)
            {
                Crosshair.IsVisible = true;
            }
            int pixelX = (int)e.MouseDevice.GetPosition(WpfPlot1).X;
            int pixelY = (int)e.MouseDevice.GetPosition(WpfPlot1).Y;
            (double coordinateX, double coordinateY) = WpfPlot1.GetMouseCoordinates();


            txtX.Text = $"{WpfPlot1.Plot.GetCoordinateX(pixelX):0.00}";
            txtY.Text = $"{WpfPlot1.Plot.GetCoordinateY(pixelY):0.00}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;
            WpfPlot1.Refresh();
        }

        /// <summary>
        /// 鼠标划出十字标隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpfPlot1_MouseLeave(object sender, MouseEventArgs e)
        {
            Crosshair.IsVisible = false;
            WpfPlot1.Refresh();
        }
        double CustomSnapFunction(double value)
        {
            // multiple of 3 between 0 and 50
            if (value < 0) return 0;
            else if (value > 2000) return 2000;
            else return (int)Math.Round(value / 3) * 3;
        }
        /// <summary>
        /// 添加标尺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAxisLine_Click(object sender, RoutedEventArgs e)
        {
            var limits = WpfPlot1.Plot.GetAxisLimits();
            AxisLineVernier axisLine = new AxisLineVernier();
            string ColorRgb;
            if ((bool)AxisLineX.IsChecked)
            {
                axisLine.AxisLineModel = WpfPlot1.Plot.AddVerticalLine(1500+limits.XSpan * AxisLineVerniers.Count);
                axisLine.AxisLineXY = "X轴";
            }
            else if ((bool)AxisLineY.IsChecked)
            {
                axisLine.AxisLineModel = WpfPlot1.Plot.AddHorizontalLine(1500+limits.YSpan * AxisLineVerniers.Count);
                axisLine.AxisLineXY = "Y轴";
            }
            axisLine.AxisLineModel.LineWidth = 2;
            axisLine.AxisLineModel.PositionLabel = true;
            axisLine.AxisLineModel.PositionLabelBackground = axisLine.AxisLineModel.Color;
            axisLine.AxisLineModel.DragEnabled = true;
            ColorRgb = System.Drawing.ColorTranslator.ToHtml(axisLine.AxisLineModel.Color);
            axisLine.LineColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorRgb));

            // different snap sytems can be created and customized 
            var SnapDisabled = new ScottPlot.SnapLogic.NoSnap1D();
            var SnapCustom = new ScottPlot.SnapLogic.Custom1D(CustomSnapFunction);
            axisLine.AxisLineModel.DragEnabled = true;
            axisLine.AxisLineModel.DragSnap = new ScottPlot.SnapLogic.Independent2D(SnapCustom, SnapDisabled);
            axisLine.AxisLineModel.Dragged += AxisLineModel_Dragged;


            AxisLineVerniers.Add(axisLine);
            WpfPlot1.Refresh();

            //axisLine.AxisLineModel.GetAxisLimits();
        }

        /// <summary>
        /// 标尺拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AxisLineModel_Dragged(object sender, EventArgs e)
        {
            VLine vLine = (VLine)sender;

            txteX.Text = vLine.X.ToString();
        }

        /// <summary>
        /// 移除标尺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelAxisLine_Click(object sender, RoutedEventArgs e)
        {
            if (AxisLineVerniers.Count == 0) return;
            AxisLineVerniers.ToList().ForEach(axisLine =>
            {
                if (axisLine.AxisLineDisabled)
                {
                    WpfPlot1.Plot.Remove(axisLine.AxisLineModel);
                    AxisLineVerniers.Remove(axisLine);
                }
            });

            WpfPlot1.Refresh();
        }

        /// <summary>
        /// 开始进行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSettingTime.Text)) return;
            int runTime = Convert.ToInt32(txtSettingTime.Text);
            m_countSecond = runTime - 1;
            tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(runTime * 1000);
            SPdataProcessing = new Task(() => RefreshOPMUI(), tokenSource.Token);
            SPdataProcessing.Start();
            m_disTimer.Start();//计时开始
            Array.Copy(myData1, 1, myData1, 0, myData1.Length - 1);
            Array.Copy(myData2, 1, myData2, 0, myData2.Length - 1);

            myData1[myData1.Length - 1] = 50;
            myData2[myData2.Length - 1] = 20;
            WpfPlot1.Refresh();

            BtnStop.IsEnabled = true;
            BtnStart.IsEnabled = false;
        }

        /// <summary>
        /// 停止模拟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            BtnStop.IsEnabled = false;
            BtnStart.IsEnabled = true;
            renderedIndex = 0;
            WpfPlot1.Refresh();
        }

        /// <summary>
        /// 模拟参数
        /// </summary>
        public async void RefreshOPMUI()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                Random rd = new Random();

                int v1 = rd.Next(0, 100);
                int v2 = rd.Next(0, 200);
                renderedIndex++;
                // double[] values = ScottPlot.DataGen.RandomWalk(rand, 2);
                Array.Copy(myData1, 1, myData1, 0, myData1.Length - 1);
                Array.Copy(myData2, 1, myData2, 0, myData2.Length - 1);

                myData1[myData1.Length - 1] = v1;
                myData2[myData2.Length - 1] = v2;
                //signalPlot1.MaxRenderIndex = renderedIndex;
                Dispatcher.Invoke(new Action(delegate
                {
                    WpfPlot1.Refresh();
                    //WpfPlot1.Plot.AxisAutoY();//自动调整Y轴范围
                   // WpfPlot1.Plot.AxisAutoX();//自动调整X轴范围
                }));


                //Thread.Sleep(50);
                await Task.Delay(50);
            }
        }
        /// <summary>
        /// 曲线1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (initflag == false)
            {
                return;
            }
            if (checkBox.IsChecked == true)
            {
                signalPlot1.IsVisible = true;
            }
            else
            {
                signalPlot1.IsVisible = false;
            }
        }

        /// <summary>
        /// 曲线2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (initflag == false)
            {
                return;
            }
            if (checkBox1.IsChecked == true)
            {
                signalPlot2.IsVisible = true;
            }
            else
            {
                signalPlot2.IsVisible = false;
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEmpty_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
