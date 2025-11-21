using BookDatabase.Models;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace BookDatabase.Windows
{

    public partial class AddAuthorForm : Window
    {
        public List<GeneralModel> Countries { get; set; }

        Database db = Database.Instace;

        public Author? EditedAuthor { get; set; }

        public AddAuthorForm(Models.Func func, Author? author = null)
        {
            InitializeComponent();
            DataContext = this;
            Countries = db.SelectNameByTableName("Countries");

           if (func == Func.Edit)
            {
                EditedAuthor = author;
                StartEdit();
            }
        }

        private void StartEdit()
        {
            if (EditedAuthor is null)
                return;

            AcceptButton.Click -= Accept;
            AcceptButton.Click += AcceptEdit;
            CountriesBox.SelectedItem = Countries.FirstOrDefault(c => c.Name == EditedAuthor.Name);
               
            List<string> list = EditedAuthor!.Name!.Split(" ").ToList();
            SurNameAuthor.Text = list.Last();
            list.RemoveAt(list.Count - 1);
            string name = string.Join("", list);
            NameAuthor.Text = name;
            AboutAuthor.Text = EditedAuthor.AboutAuthor;
            DateAuthor.Text = EditedAuthor.DateOfBirth;

        }

        private void AcceptEdit(object sender, RoutedEventArgs e)
        {
            if (!Control())
            {
                return;
            }
            DateTime date = DateTime.Parse(DateAuthor.Text);
            db.UpdateAuthor(EditedAuthor!.Id, NameAuthor.Text, SurNameAuthor.Text, date, ((GeneralModel)CountriesBox.SelectedItem).Name, AboutAuthor.Text);
            EditedAuthor = db.SelectAuthor(NameAuthor.Text + " " + SurNameAuthor.Text);

            this.Close();
        }

        public void Return(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsEmpty())
            {
                this.Close();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                  "Opravdu se chcete vrátit?\nVaše vyplněné pole budou ztraceny!",
                  "Confirmation",
                  MessageBoxButton.YesNo,
                  MessageBoxImage.Question
              );

                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
                
        }

        private void DatePicker_DateValidationError(object sender, DatePickerDateValidationErrorEventArgs e)
        {
            MessageBox.Show("nesprávně zadané datum");
            DateAuthor.Background = Brushes.Red;
        }

        private void MyDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateAuthor.Background = Brushes.White;
        }

        private bool AreAllFieldsEmpty()
        {
            return DateAuthor.SelectedDate == null
                && string.IsNullOrWhiteSpace(AboutAuthor.Text)
                && string.IsNullOrWhiteSpace(NameAuthor.Text)
                && string.IsNullOrWhiteSpace(SurNameAuthor.Text)
                && CountriesBox.SelectedItem == null;
        }

        private bool Control()
        {
            if (DateAuthor.SelectedDate == null)
            {
                MessageBox.Show("nesprávně zadané datum");
                DateAuthor.Background = Brushes.Red;
                return false;
            }
            if (String.IsNullOrWhiteSpace(AboutAuthor.Text))
            {
                MessageBox.Show("vyplňte pole o autorovi");
                return false;
            }
            if (String.IsNullOrWhiteSpace(NameAuthor.Text))
            {
                MessageBox.Show("vyplňte autorovo jméno");
                return false;
            }
            if (String.IsNullOrWhiteSpace(SurNameAuthor.Text))
            {
                MessageBox.Show("vyplňte autorovo přijímení");
                return false;
            }
            if (CountriesBox.SelectedItem == null)
            {
                MessageBox.Show("vyberte zemi původu");
                return false;
            }

            List<Author> list = db.SelectAuthorWithSearch(NameAuthor.Text + SurNameAuthor.Text);
            if (list.Count > 0)
            {
                MessageBox.Show("Tento Autor již existuje!");
                return false;
            }

            return true;
        }

        private void Accept(object sender, RoutedEventArgs e)
        {

            if (!Control())
            {
                return;
            }

            db.InsertAuthor(NameAuthor.Text ,SurNameAuthor.Text, ((GeneralModel)CountriesBox.SelectedItem).Name, (DateTime)DateAuthor.SelectedDate!, AboutAuthor.Text);
            this.Close();


        }
    }
}
