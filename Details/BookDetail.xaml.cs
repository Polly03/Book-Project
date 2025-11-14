using BookDatabase.Models;
using DevExpress.Utils.CommonDialogs.Internal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace BookDatabase.Details
{
    /// <summary>
    /// Interaction logic for BookDetail.xaml
    /// </summary>
    public partial class BookDetail : UserControl
    {
  
        public Database db = Database.Instance;
        public Book SelectedBook { get; set; }
        public BookDetail(string title)
        {
            InitializeComponent();
            this.DataContext = this;

            Book book = db.SelectBook(title, 1);
            this.SelectedBook = book;

            BookImage.Source = book.Image;
            BookTitleBox.Text = book.Name;
            AuthorNameBox.Text = book.Author;
            DescriptionBox.Text = book.Description;
            RatingBox.Text = book.Rating;
            GenreBox.Text = book.Genre;
            LengthBox.Text = book.Length.ToString();
            TypeBox.Text = book.Type;
            LanguageBox.Text = book.Langueage;
            PublisherBox.Text = book.Publisher; 
            EANBox.Text = book.EAN; 
            ISBNBox.Text = book.ISBN;

        }

        private void BackToBooksWindow(object sender, System.Windows.RoutedEventArgs e)
        {

            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        private void EditBook(object sender, RoutedEventArgs e)
        {
            AddBookForm win = new AddBookForm("edit", SelectedBook);
            win.ShowDialog();
        }

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
