using System.Windows.Controls;
using System.Windows.Media;

namespace PrismAppDemo.Views.PageView
{
    /// <summary>
    /// Interaction logic for ViewA
    /// </summary>
    public partial class ViewA : UserControl
    {
        public ViewA()
        {
            InitializeComponent();
        }

        private void pocker_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            //txtColor.Background = new SolidColorBrush((Color)pocker.SelectedColor);
            //pocker.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void txtColor_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            popAdd.IsOpen = false;
            popAdd.IsOpen = true;
            // pocker.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
