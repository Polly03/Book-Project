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


        // properties for setting, getting and updating automaticly filters for book cards AUTHORS
        public ObservableCollection<FilterOption> AuthorsFilter { get; set; }
        private string fSearchAuthorsFilter = string.Empty;
        public string SearchAuthorsFilter
        {
            get => fSearchAuthorsFilter;
            set
            {
                fSearchAuthorsFilter = value;
                AuthorsFilter.Clear();
                AuthorsFilter = filter("Authors", value);
                OnPropertyChanged(nameof(AuthorsFilter));
            }
        }



        // properties for setting, getting and updating automaticly filters for book cards GENRES
        public ObservableCollection<FilterOption> GenresFilter { get; set; }
        private string fSearchGenresFilter = string.Empty;
        public string SearchGenresFilter
        {
            get => fSearchGenresFilter;
            set
            {
                fSearchGenresFilter = value;
                GenresFilter.Clear();
                GenresFilter = filter("Genres", value);
                OnPropertyChanged(nameof(GenresFilter));
            }
        }



        // properties for setting, getting and updating automaticly filters for book cards LANGUAGES
        public ObservableCollection<FilterOption> LanguagesFilter { get; set; }
        private string fSearchLanguagesFilter = string.Empty;
        public string SearchLanguagesFilter
        {
            get => fSearchLanguagesFilter;
            set
            {
                fSearchGenresFilter = value;
                LanguagesFilter.Clear();
                LanguagesFilter = filter("Languages", value);
                OnPropertyChanged(nameof(LanguagesFilter));
            }
        }




        // properties for setting, getting and updating automaticly filters for book cards PUBLISHERS
        public ObservableCollection<FilterOption> PublishersFilters { get; set; }
        private string fSearchPublishersFilter = string.Empty;
        public string SearchPublishersFilter
        {
            get => fSearchPublishersFilter;
            set
            {
                fSearchGenresFilter = value;
                PublishersFilters.Clear();
                PublishersFilters = filter("Publishers", value);
                OnPropertyChanged(nameof(PublishersFilters));
            }
        }


        // method for selecting only filters in substring
        private ObservableCollection<FilterOption> filter(string table, string txt)
        {
            return new ObservableCollection<FilterOption>(
                db.SelectNameByTableName(table).Where(elem => elem.Name.ToLower().Contains(txt.ToLower()))
                                               .Select(elem => new FilterOption { Name = elem.Name })
            );
        }

        // evennt method for reacting on changed property
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


            AuthorsFilter = FillCheckBoxes("Authors");
            GenresFilter = FillCheckBoxes("Genres");
            LanguagesFilter = FillCheckBoxes("Languages");
            PublishersFilters = FillCheckBoxes("Publishers");

            AuthorsFilter = new ObservableCollection<FilterOption>(AuthorsFilter);
            GenresFilter = new ObservableCollection<FilterOption>(GenresFilter);
            LanguagesFilter = new ObservableCollection<FilterOption>(LanguagesFilter);
            PublishersFilters = new ObservableCollection<FilterOption>(PublishersFilters);
            // filling properties

            

            AuthorsFilter = SetPropertyChange(AuthorsFilter);
            PublishersFilters = SetPropertyChange(PublishersFilters);
            GenresFilter = SetPropertyChange(GenresFilter);
            LanguagesFilter = SetPropertyChange(LanguagesFilter);
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
            DelFilters(AuthorsFilter);
            DelFilters(GenresFilter);
            DelFilters(PublishersFilters);
            DelFilters(AuthorsFilter);
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
            var authors = DoFilter(AuthorsFilter);
            var genre = DoFilter(GenresFilter);
            var languages = DoFilter(LanguagesFilter);
            var publishers = DoFilter(PublishersFilters);

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
            else if (s == "ABCdsc") { column = "books.name"; way = "desc"; }
            else if (s == "LENasc") { column = "books.length"; way = "asc"; }
            else if (s == "LENdsc") { column = "books.length"; way = "desc"; }


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
