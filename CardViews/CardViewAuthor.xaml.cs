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
        public static readonly DependencyProperty NameOfProperty =
            DependencyProperty.Register("NameOf", typeof(string), typeof(CardViewAuthor), new PropertyMetadata(""));

        public string NameOf
        {
            get => (string)GetValue(NameOfProperty);
            set => SetValue(NameOfProperty, value);
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
                var author = fe.DataContext as Authors;
                if (author != null)
                {
                    ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorDetail(author.NameOf);
                }
            }
        }

    }
}
