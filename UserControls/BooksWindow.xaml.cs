using BookDatabase.Models;
using BookDatabase.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookDatabase
{

    public partial class BooksWindow : UserControl
    {
    // class for showing and filtering books

        ObservableProperties p = ObservableProperties.Instance;
        // getting properties used for binding data in xaml and getting Irremovalbe repetetive code out of this class

        Database db = Database.Instance;
        // class for working with database

        public BooksWindow()
        {
            InitializeComponent();

            DataContext = p;
            SelectCards();


            p.Authors = FillCheckBoxes("Authors");
            p.Genres = FillCheckBoxes("Genres");
            p.Languages = FillCheckBoxes("Languages");
            p.Publishers = FillCheckBoxes("Publishers");

            p.FilteredAuthors = new ObservableCollection<FilterOption>(p.Authors);
            p.FilteredGenres = new ObservableCollection<FilterOption>(p.Genres);
            p.FilteredLanguages = new ObservableCollection<FilterOption>(p.Languages);
            p.FilteredPublishers = new ObservableCollection<FilterOption>(p.Publishers);
            // filling properties

            p.Authors = SetPropertyChange(p.Authors);
            p.Publishers = SetPropertyChange(p.Publishers);
            p.Genres = SetPropertyChange(p.Genres);
            p.Languages = SetPropertyChange(p.Languages);
            // adding event method when filter is checked or unchecked

        }

        // method for adding element in list property of ApplyFilter
        private ObservableCollection<FilterOption> SetPropertyChange(ObservableCollection<FilterOption> list)
        {
            foreach (var elem in list)
            {
                elem.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }
            return list;
        }

        // getting data from database and filling filter by name
        private ObservableCollection<FilterOption> FillCheckBoxes(string txt)
        {
            ObservableCollection<FilterOption> filters = new ObservableCollection<FilterOption>();

            List<string> list = db.SelectTableByName(txt);
            foreach (string elem in list)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                filters.Add(option);
            }

            return filters;
        }

        // start method for showing all books or refresh books
        private void SelectCards()
        {
            p.MyItems = new ObservableCollection<BooksData>();
            List<Tuple<byte[], string, string, string>> list = db.SelectAllBooks();

            foreach (var item in list)
            {

                p.MyItems.Add(new BooksData(GetBM(item.Item1), item.Item2, item.Item3, item.Item4));

            }
        }

        // decoding byte[] data of image to BitmapImage to show it in Cards
        private BitmapImage GetBM(byte[] data)
        {
            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(data))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;

                bitmap.DecodePixelWidth = 125; 
                bitmap.DecodePixelHeight = 150; 

                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }

            bitmap.Freeze(); 
            return bitmap;
        }

        // Method for reseting filters
        private void DeleteFilters(object sender, RoutedEventArgs e)
        {
            DelFilters(p.Authors);
            DelFilters(p.FilteredGenres);
            DelFilters(p.FilteredPublishers);
            DelFilters(p.FilteredAuthors);
        }

        private void DelFilters(ObservableCollection<FilterOption> list)
        {
            foreach (var filter in list)
            {
                filter.IsSelected = false;
            }
        }

        // Method called when someone check the filter
        private void ApplyFilter()
        {
            var authors = DoFilter(p.Authors);
            var genres = DoFilter(p.Genres);
            var languages = DoFilter(p.Languages);
            var publishers = DoFilter(p.Publishers);

            List<Tuple<byte[], string, string, string>> list = db.SelectBooksByFilters(authors, genres, languages, publishers);

            p.MyItems.Clear();
            foreach (var elem in list)
            {
                p.MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }
        }

        // getting filters in correct order to work like a list
        private string DoFilter(ObservableCollection<FilterOption> list)
        {
            List<string> selected = GetSelected(list);
            return selected.Count == 0 ? "" :
                string.Join(",", selected.Select(a => $"'{a.Replace("'", "''")}'"));
        }

        // select name of selected items
        private List<string> GetSelected(ObservableCollection<FilterOption> list)
        {
            return list.Where(a => a.IsSelected).Select(a => a.Name).ToList() as List<string>;
        }


        // method when user select order of books by which element and in which way
        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            string column = "";
            string way = "";
            if (s == "ABCasc") { column = "books.name"; way = "asc"; }
            if (s == "ABCdsc") { column = "books.name"; way = "desc"; }
            if (s == "LENasc") { column = "books.length"; way = "asc"; }
            if (s == "LENdsc") { column = "books.length"; way = "desc"; }


            List<Tuple<byte[], string, string, string>> list = db.OrderBooks(column, way);
            p.MyItems.Clear();
            foreach (var elem in list)
            {
                p.MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }
        }

        // click method for button which start searching if substring in searchbar is in some book names or author names
        private void StartSearchBook(object sender, EventArgs e)
        {
            List<Tuple<Byte[], string, string, string>> list = db.SelectBooksWithSearch(searchBar.Text);
            p.MyItems.Clear();
            foreach (var elem in list)
            {
                p.MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }

        }

        // metod for opening form for adding book
        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            AddBookForm win = new AddBookForm();
            win.ShowDialog();
        }

        // click method for redirect to authorsWindow
        private void OpenAuthors(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorsWindow();
        }
    }

    
}
