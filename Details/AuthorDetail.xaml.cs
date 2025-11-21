using BookDatabase.Models;
using BookDatabase.Windows;
using System.Windows;
using System.Windows.Controls;

namespace BookDatabase.Details
{

    /// <summary>
    /// 
    /// CLASS for selecting, showing and editing details about author
    /// 
    /// </summary>
    public partial class AuthorDetail : UserControl
    {
        private Database db = Database.Instace; 

        // Property for keeping the selected author and its details
        public Author Author {  get; set; }  

        // from BooksWindow or Author window i am getting name of author i clicked on
        public AuthorDetail(string AuthorNameFull)
        {
            Author = db.SelectAuthor(AuthorNameFull);
            InitializeComponent();
            DataContext = this;
            ShowAuthor();

        }

        // method for puting selected author to the gui
        private void ShowAuthor()
        {
            AuthorName.Text = Author.Name;
            AuthorCountry.Text = Author.Country;
            AuthorDateOfBirth.Text = Author.DateOfBirth;
            AboutAuthor.Text = Author.AboutAuthor;
        }

        // return from details to book selection
        private void Return(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorWindow();
        }

        // opening AuthorForm adjusted for editing author and also letting know about this this window for updating its Author
        private void EditAuthor(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm(Func.Edit, Author);

            win.Closed += (s, eArgs) =>
            {
                Author = db.SelectAuthor(win.EditedAuthor!.Name);
                ShowAuthor();
            };

            win.ShowDialog();
        }

        // if author have no books, then delete author
        private void DeleteAuthor(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
               "Opravdu chcete smazat Autora",
               "Confirmation",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question
           );

            if (result == MessageBoxResult.Yes)
            {
                int i = db.DeleteAuthor(Author.Name);
                if (i == 1)
                {
                    MessageBox.Show("Autor má v aplikaci knihu, nemuže být smazán");
                }
                else
                {
                    MessageBox.Show("Autor smazán");
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorWindow();
                }

            }
        }
    }
}
