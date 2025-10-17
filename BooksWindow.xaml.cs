using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookDatabase
{
    /// <summary>
    /// Interaction logic for BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : UserControl

    {
        public ObservableCollection<CardData> MyItems { get; set; }

        public BooksWindow()
        {
            InitializeComponent();

            MyItems = new ObservableCollection<CardData>();

            DataContext = this;

            for (int i = 0; i < 5; i++)
            {
                MyItems.Add(new CardData { Width = 200, Height = 400 });
            }

            


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyItems.Add(new CardData { Width = 200, Height = 400 });
        }
    }

    public class CardData
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
