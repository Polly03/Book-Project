using BookDatabase.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BookDatabase
{

    public partial class BooksWindow : UserControl, INotifyPropertyChanged
    {
    // class for showing and filtering books

        Database db = Database.Instance;
        // class for working with database

        // collection for BookCards


        public ObservableCollection<BooksData> BookCards { get; set; }

        /* NOVE POJMENOVANI */
        public ObservableCollection<FilterOption> AuthorFilters { get; set; }
        private string fSearchAuthorFilter = string.Empty;
        public string SearchAuthorFilter
        {
            get => fSearchAuthorFilter;
            set
            {
                fSearchAuthorFilter = value;
                AuthorFilters.Clear();
                AuthorFilters = filter("Authors", value);
                OnPropertyChanged(nameof(AuthorFilters));
            }
        }
        /* NOVE POJMENOVANI*/

        public ObservableCollection<FilterOption> FilteredGenres { get; set; }
        private string fSearchTextGenre = string.Empty;
        public string SearchTextGenre
        {
            get => fSearchTextGenre;
            set
            {
                fSearchTextGenre = value;
                FilteredGenres.Clear();
                FilteredGenres = filter("Genres", value);
                OnPropertyChanged(nameof(FilteredGenres));
            }
        }


        public ObservableCollection<FilterOption> FilteredLanguages { get; set; }
        private string fSearchTextLanguage = string.Empty;
        public string SearchTextLanguage
        {
            get => fSearchTextLanguage;
            set
            {
                fSearchTextGenre = value;
                FilteredLanguages.Clear();
                FilteredLanguages = filter("Languages", value);
                OnPropertyChanged(nameof(FilteredLanguages));
            }
        }

        public ObservableCollection<FilterOption> FilteredPublishers { get; set; }
        private string fSearchTextPublisher = string.Empty;
        public string SearchTextPublisher
        {
            get => fSearchTextPublisher;
            set
            {
                fSearchTextGenre = value;
                FilteredPublishers.Clear();
                FilteredPublishers = filter("Publishers", value);
                OnPropertyChanged(nameof(FilteredPublishers));
            }
        }

        private ObservableCollection<FilterOption> filter(string table, string txt)
        {
            return new ObservableCollection<FilterOption>(
                db.SelectNameByTableName(table).Where(elem => elem.Name.ToLower().Contains(txt.ToLower()))
                                               .Select(elem => new FilterOption { Name = elem.Name })
            );
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
          


    

        public BooksWindow()
        {
            InitializeComponent();

            DataContext = this;
            SelectCards();


            AuthorFilters = FillCheckBoxes("Authors");

            FilteredGenres = FillCheckBoxes("Genres");
            FilteredLanguages = FillCheckBoxes("Languages");
            FilteredPublishers = FillCheckBoxes("Publishers");

            AuthorFilters = new ObservableCollection<FilterOption>(AuthorFilters);
            FilteredGenres = new ObservableCollection<FilterOption>(FilteredGenres);
            FilteredLanguages = new ObservableCollection<FilterOption>(FilteredLanguages);
            FilteredPublishers = new ObservableCollection<FilterOption>(FilteredPublishers);
            // filling properties

            

            AuthorFilters = SetPropertyChange(AuthorFilters);
            FilteredPublishers = SetPropertyChange(FilteredPublishers);
            FilteredGenres = SetPropertyChange(FilteredGenres);
            FilteredLanguages = SetPropertyChange(FilteredLanguages);
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
            List<GeneralModel> list = db.SelectNameByTableName(txt);

            foreach (GeneralModel elem in list)
            {
                FilterOption option = new FilterOption();
                option.Name = elem.Name;
                filters.Add(option);
            }

            return filters;
        }

        private void FillItems(List<BooksData> list)
        {
            BookCards.Clear();
            foreach (var elem in list)
            {
                BookCards.Add(elem);
            }
        }

        // start method for showing all books or refresh books
        private void SelectCards()
        {
            BookCards = new ObservableCollection<BooksData>();
            List<BooksData> list = db.SelectAllBooks();

           FillItems(list);
        }

        // decoding byte[] data of image to BitmapImage to show it in Cards
        

        // Method for reseting filters
        private void DeleteFilters(object sender, RoutedEventArgs e)
        {
            DelFilters(AuthorFilters);
            DelFilters(FilteredGenres);
            DelFilters(FilteredPublishers);
            DelFilters(AuthorFilters);
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
            var authors = DoFilter(AuthorFilters);
            var genre = DoFilter(FilteredGenres);
            var languages = DoFilter(FilteredLanguages);
            var publishers = DoFilter(FilteredPublishers);

            List<BooksData> list = db.SelectBooksByFilters(authors, genre, languages, publishers);

            FillItems(list);
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


            List<BooksData> list = db.OrderBooks(column, way);
            FillItems(list);
        }

        // click method for button which start searching if substring in searchbar is in some book names or author names
        private void StartSearchBook(object sender, EventArgs e)
        {
            List<BooksData> list = db.SelectBooksWithSearch(searchBar.Text);
            FillItems(list);
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
