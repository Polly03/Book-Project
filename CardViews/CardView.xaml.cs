using BookDatabase.Details;
using BookDatabase.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



namespace BookDatabase
{

    public partial class CardView : UserControl
    {
        public CardView()
        {
            InitializeComponent();
        }

        // properties for correctly working input when CardView class will be used as element in other xaml file
        public static readonly DependencyProperty ImageSourceProperty =
                DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CardView), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CardView), new PropertyMetadata(""));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(CardView), new PropertyMetadata(""));

        public string Author
        {
            get => (string)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        public static readonly DependencyProperty GenreProperty =
            DependencyProperty.Register("Genre", typeof(string), typeof(CardView), new PropertyMetadata(""));

        public string Genre
        {
            get => (string)GetValue(GenreProperty);
            set => SetValue(GenreProperty, value);
        }

        // methods ready for showing details of book and author
        private void ShowBook(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
       
                var book = fe.DataContext as BooksData; 
                if (book != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new BookDetail(book.Title);
                }
            }
        }

        private void ShowAuthor(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
                var author = fe.DataContext as BooksData;
                if (author != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorDetail(author.Author);
                }
            }
        }

    }
}
