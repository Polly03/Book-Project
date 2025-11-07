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
using System.Windows.Navigation;
using System.Windows.Shapes;


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
        public void ShowBook(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Getting book");
        }

        public void ShowAuthor(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Getting Author");
        }

    }
}
