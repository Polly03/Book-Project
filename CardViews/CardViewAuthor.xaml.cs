using BookDatabase.Details;
using BookDatabase.Models;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;


namespace BookDatabase
{

    public partial class CardViewAuthor : UserControl
    {


        public CardViewAuthor()
        {
            InitializeComponent();
        }

        // properties for working card when used as element in other xaml
        public static readonly DependencyProperty AuthorNameProperty =
            DependencyProperty.Register("AuthorName", typeof(string), typeof(CardViewAuthor), new PropertyMetadata(""));

        public string AuthorName
        {
            get => (string)GetValue(AuthorNameProperty);
            set => SetValue(AuthorNameProperty, value);
        }

        public static readonly DependencyProperty DateOfBirthProperty =
            DependencyProperty.Register("DateOfBirth", typeof(string), typeof(CardViewAuthor), new PropertyMetadata(""));

        public string DateOfBirth
        {
            get => (string)GetValue(DateOfBirthProperty);
            set => SetValue(DateOfBirthProperty, value);
        }

        public static readonly DependencyProperty CountryProperty =
            DependencyProperty.Register("Country", typeof(string), typeof(CardViewAuthor), new PropertyMetadata(""));

        public string Country
        {
            get => (string)GetValue(CountryProperty);
            set => SetValue(CountryProperty, value);
        }

        private void ShowAuthor(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
                var author = fe.DataContext as Author;
                if (author != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorDetail(author.Name);
                }
            }
        }

    }
}
