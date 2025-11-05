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

    public partial class CardViewAuthor : UserControl
    {
        public CardViewAuthor()
        {
            InitializeComponent();
        }

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
    }
}
