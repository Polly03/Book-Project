using BookDatabase.Models;
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
using System.Windows.Shapes;

namespace BookDatabase.Windows
{
    /// <summary>
    /// Interaction logic for AddAuthorForm.xaml
    /// </summary>
    public partial class AddAuthorForm : Window
    {
        public List<Authors> Authors { get; set; }
        private List<Authors> _authors;
        private string _win;
        public AddAuthorForm(string win)
        {

            _win = win;
            InitializeComponent();
            DataContext = this;
            Authors = new List<Authors>();

            Database database = new Database();
            List<string> list = database.SelectTableByName("Countries");

            foreach (string item in list)
            {
                Authors.Add(new Authors(150, 150, item, item, item));
            }
        }

        public void Return(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DatePicker_DateValidationError(object sender, DatePickerDateValidationErrorEventArgs e)
        {
            MessageBox.Show("NEPLATNĚ ZADANÉ DATUM");
            DateAuthor.Background = Brushes.Red;
        }

        private void MyDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateAuthor.Background = Brushes.White;
        }

        private void Accept(object sender, RoutedEventArgs e)
        {
            if (DateAuthor.Background == Brushes.Red)
            {
                MessageBox.Show("nesprávně zadané datum");
                return;
            }
            if (String.IsNullOrWhiteSpace(AboutAuthor.Text))
            {
                MessageBox.Show("vyplňte pole o autorovi");
                return;
            }
            if (String.IsNullOrWhiteSpace(NameOfAuthor.Text))
            {
                MessageBox.Show("vyplňte autorovo jméno");
                return;
            }
            if (CountriesBox.SelectedItem == null)
            {
                MessageBox.Show("vyberte zemi původu");
                return;
            }

            Database db = new Database();
            db.InsertAuthor(NameOfAuthor.Text, CountriesBox.SelectedItem.ToString(), (DateTime)DateAuthor.SelectedDate, AboutAuthor.Text);
            this.Close();


        }
    }
}
