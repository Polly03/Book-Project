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
using System.Windows.Shapes;

namespace BookDatabase
{
    /// <summary>
    /// Interakční logika pro test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            if (SmallMenu.Visibility == Visibility.Visible)
            {
                SmallMenu.Visibility = Visibility.Collapsed;
                LargeMenu.Visibility = Visibility.Visible;
            }
            else
            {
                SmallMenu.Visibility = Visibility.Visible;
                LargeMenu.Visibility = Visibility.Collapsed;
            }
        }
    }
}
