using BookDatabase.Models;
using System.Windows;
using System.Windows.Controls;


namespace BookDatabase.Details
{

    /// <summary>
    /// 
    /// CLASS for selecting, showing and editing details about book
    /// 
    /// </summary>


    public partial class BookDetail : UserControl
    {
      
        public Database db = Database.Instace;
        public Book SelectedBook { get; set; }
        // book selected for details
        public BookDetail(string title)
        {
            InitializeComponent();
            this.DataContext = this;
            Book book = db.SelectBook(title, 1);
            this.SelectedBook = book;

            ShowBook();

        }

        // method for showing all details in gui selected book
        private void ShowBook()
        {

            BookImage.Source = SelectedBook.Image;
            BookTitleBox.Text = SelectedBook.Name;
            AuthorNameBox.Text = SelectedBook.Author;
            DescriptionBox.Text = SelectedBook.Description;
            RatingBox.Text = SelectedBook.Rating;
            GenreBox.Text = SelectedBook.Genre;
            LengthBox.Text = SelectedBook.Length.ToString();
            TypeBox.Text = SelectedBook.Type;
            LanguageBox.Text = SelectedBook.Langueage;
            PublisherBox.Text = SelectedBook.Publisher;
            EANBox.Text = SelectedBook.EAN;
            ISBNBox.Text = SelectedBook.ISBN;
        }

        // returning from details
        private void BackToBooksWindow(object sender, RoutedEventArgs e)
        {

            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        // editing book with Adjusted user control AddBookForm for editing book
        private void EditBook(object sender, RoutedEventArgs e)
        {
            AddBookForm win = new AddBookForm("edit", SelectedBook);
        
            win.Closed += (s, eArgs) =>
            {
                SelectedBook = db.SelectBook(win.EditBook.Name, 1);
                ShowBook();
            };

            win.ShowDialog();
        }

        
        // deleting book if user click on yes
        private void DeleteBook(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Opravdu chcete smazat knihu?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                db.DeleteBook(SelectedBook.Name, SelectedBook.Id);
                MessageBox.Show("kniha smazána");
                BackToBooksWindow(this, e);
            }
        }
    }
}
