using BookDatabase.Windows;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookDatabase
{

    public partial class AddBookForm : Window
    {
        public AddBookForm()
        {
            InitializeComponent();
        }

        private void Length_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
        public void Return(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddAuthorButton(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm("Book");
            win.ShowDialog();
        }
    }
}
