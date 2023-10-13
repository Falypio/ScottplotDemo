using System.Collections.Generic;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using Microsoft.Win32;
using Prism.Services.Dialogs;
using System.Windows.Data;
using PrismAppDemo.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PrismAppDemo.Views.PageView
{
    /// <summary>
    /// Interaction logic for ViewC
    /// </summary>
    public partial class ViewC : UserControl
    {
        public GridLength m_WidthCache;

        DataTable dt = new DataTable();



        public Dictionary<string, ObservableCollection<double>> Dic;

        public ViewC()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ViewC_Loaded);


            //dt.Columns.Add(new DataColumn("Column1"));
            //dt.Columns.Add(new DataColumn("Column2"));

            //DataRow dr;
            //for (int i = 0; i < 5; i++)
            //{
            //    dr = dt.NewRow();
            //    for (int columIndex = 0; columIndex < dt.Columns.Count; columIndex++)
            //        dr[columIndex] = i.ToString() + " - " + columIndex.ToString();
            //    dt.Rows.Add(dr);
            //}

            //dataGrid.ItemsSource = dt.DefaultView;
            AddDataGrid();
            AddDataGrid2();
        }

        private void AddDataGrid()
        {
            for (int j = 0; j < 10; j++)
            {
                dt.Columns.Add(new DataColumn($"Column{j}"));
            }
            DataRow dr;
            for (int i = 0; i < 3; i++)
            {
                dr = dt.NewRow();
                for (int columIndex = 0; columIndex < dt.Columns.Count; columIndex++)
                    dr[columIndex] = i.ToString() + " - " + columIndex.ToString();
                dt.Rows.Add(dr);
            }
            dataGrid.ItemsSource = dt.DefaultView;
        }
        int cc = 0;
        private void UpdateDataGrid()
        { 
            for (int j = 1;j < dataGrid.Columns.Count ; j++) 
            {
                for (int i = 0; i < dataGrid.Items.Count ; i++)
                {
                    var ds = j;
                    var cd = i;
                    dt.Rows[i][j] =$"{j}-{i}:{cc}";
                }
            }
            cc++;
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dt.DefaultView;
        }
        /// <summary>
        /// 更新DateGrid数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDateGrid_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
            UpdateDataGrid2();
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dt.NewRow();
            for (int columIndex = 0; columIndex < dt.Columns.Count; columIndex++)
                dr[columIndex] = "New Row - " + columIndex.ToString();
            dt.Rows.Add(dr);
        }

        
        int newColumnIndex = 1;
        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            dt.Columns.Add(new DataColumn("New Column" + newColumnIndex++));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][dt.Columns.Count - 1] = i.ToString() + " - New Column";
            }
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dt.DefaultView;
        }


        private void AddDataGrid2()
        {
            Dic = new Dictionary<string, ObservableCollection<double>>();
            Binding binding = new Binding() { Source = Dic };
            this.dg.SetBinding(DataGrid.ItemsSourceProperty, binding);

            Dic.Add("X1", new ObservableCollection<double> { 12, 23, 33, 123 });
            Dic.Add("X2", new ObservableCollection<double> { 12, 23, 33, 123 });
            Dic.Add("X3", new ObservableCollection<double> { 23, 12, 54, 231 });

            int count = 0;
            foreach (ObservableCollection<double> lst in Dic.Values)
            {
                if (lst.Count > count)
                {

                    for (int i = count; i < lst.Count; i++)
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.Header = "name" + i;
                        column.Binding = new Binding(string.Format("Value[{0}]", i));
                        dg.Columns.Add(column);
                    }
                    count = lst.Count;
                }
            }
        }

        private void UpdateDataGrid2()
        {
            foreach (ObservableCollection<double> lst in Dic.Values)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        lst[i] = -1 *( i +cc);
                    }
                    else
                    {
                        lst[i] = (1 * i) + cc;
                    }
                   
                }
            }
        }

        public void ViewC_Loaded(object sender, RoutedEventArgs e)
        {
            //获取GridSplitterr的cotrolTemplate中的按钮btn，必须在Loaded之后才能获取到
            Button btnGrdSplitter = gsSplitterr.Template.FindName("btnExpend", gsSplitterr) as Button;
            if (btnGrdSplitter != null)
                btnGrdSplitter.Click += new RoutedEventHandler(btnGrdSplitter_Click);
        }

        public void btnGrdSplitter_Click(object sender, RoutedEventArgs e)
        {
            GridLength temp = grdWorkbench.ColumnDefinitions[0].Width;
            GridLength def = new GridLength();
            if (temp.Equals(def))
            {
                //恢复
                grdWorkbench.ColumnDefinitions[0].Width = m_WidthCache;
            }
            else
            {
                //折叠
                m_WidthCache = grdWorkbench.ColumnDefinitions[0].Width;
                grdWorkbench.ColumnDefinitions[0].Width = def;
            }
        }
    }
}
