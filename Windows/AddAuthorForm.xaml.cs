using BookDatabase.Models;
using Microsoft.EntityFrameworkCore.Metadata;
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

    public partial class AddAuthorForm : Window
    {
        public List<Authors> Authors { get; set; }
        private string _win;
        public event Action? AuthorAdded;

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
                Authors.Add(new Authors(item, item, item));
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
            if (String.IsNullOrWhiteSpace(SurnameOfAuthor.Text))
            {
                MessageBox.Show("vyplňte autorovo přijímení");
                return;
            }
            if (CountriesBox.SelectedItem == null)
            {
                MessageBox.Show("vyberte zemi původu");
                return;
            }

            Database db = new Database();
            List<Tuple<string, DateTime, string>> list = db.SelectAuthorWithSearch(NameOfAuthor.Text);
            if (list.Count > 0)
            {
                MessageBox.Show("Tento Autor již existuje!");
                return;
            }




            db.InsertAuthor(NameOfAuthor.Text ,SurnameOfAuthor.Text, ((Authors)CountriesBox.SelectedItem).Country, (DateTime)DateAuthor.SelectedDate!, AboutAuthor.Text);
            AuthorAdded?.Invoke();
            this.Close();


        }
    }
}
