using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PrismAppDemo.Models
{
    public class MenuCless : BindableBase
    {
        private string m_menuName;
        public string MenuName
        {
            get { return m_menuName; }
            set { SetProperty(ref m_menuName, value); }
        }
        private string m_Icon;
        public string Icon
        {
            get { return m_Icon; }
            set { SetProperty(ref m_Icon, value); }
        }
        private Brush m_menuColor;
        public Brush MenuColor
        {
            get { return m_menuColor; }
            set { SetProperty(ref m_menuColor, value); }
        }

        private string m_nameSpace;
        /// <summary>
        /// 菜单命名空间
        /// </summary>
        public string NameSpace
        {
            get { return m_nameSpace; }
            set { SetProperty(ref m_nameSpace, value); }
        }
    }
}
