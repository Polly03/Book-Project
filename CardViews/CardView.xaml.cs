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
        public static readonly DependencyProperty ImageProperty =
                DependencyProperty.Register("Image", typeof(ImageSource), typeof(CardView), new PropertyMetadata(null));

        public Image Image
        {
            get => (Image)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
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

        // methods event for opening UserControl with Details of book clicked on bookCard
        private void ShowBook(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
       
                var book = fe.DataContext as Book; 
                if (book != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new BookDetail(book.Name);
                }
            }
        }

        // methods event for opening UserControl with Details of Author clicked on BookCard
        private void ShowAuthor(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
                var book = fe.DataContext as Book; 
                if (book != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorDetail(book.Author); 
                }
            }
        }

    }
}
