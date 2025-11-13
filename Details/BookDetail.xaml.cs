using BookDatabase.Models;
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
        public string Title { get; set; }
        public Database db = Database.Instance;
        public BitmapImage ImageSource { get; set; }
        public BookDetail(string title)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Title = title;

            Book book = db.SelectBook(title);
            BookTitleBox.Text = book.Name;
            AuthorNameBox.Text = book.Author;
            DescriptionBox.Text = book.Description;
            RatingBox.Text = book.Rating;
            ImageSource = book.Image;
            GenreBox.Text = book.Genre;
            TypeBox.Text = book.Type;
            LanguageBox.Text = book.Type;
            PublisherBox.Text = book.Publisher; 
            EANBox.Text = book.EAN; 
            ISBNBox.Text = book.ISBN;
        }
    }
}
