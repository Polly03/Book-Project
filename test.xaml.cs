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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookDatabase
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : UserControl
    {
        public test()
        {
            InitializeComponent();

            MyItems = new ObservableCollection<CardData>();

            DataContext = this;

            for (int i = 0; i < 15; i++)
            {
                MyItems.Add(new CardData { Width = 200, Height = 400 });
            }
        }

        public ObservableCollection<CardData> MyItems { get; set; }

        

        public class CardData
        {
            public double Width { get; set; }
            public double Height { get; set; }
        }
    }
}
