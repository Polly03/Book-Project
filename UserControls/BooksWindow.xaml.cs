using BookDatabase.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BookDatabase
{
    // class for showing and filtering books
    public partial class BooksWindow : UserControl, INotifyPropertyChanged
    {
       

        // class for working with database    
        Database db = Database.Instace;
        
        // collection for BookCards
        public ObservableCollection<Book> BookCards { get; set; }


        // properties for setting, getting and updating automaticly filters for book cards Author
        public ObservableCollection<FilterOption> AuthorFilter { get; set; }
        private string fSearchAuthorFilter = string.Empty;
        public string SearchAuthorFilter
        {
            get => fSearchAuthorFilter;
            set
            {
                fSearchAuthorFilter = value;
                AuthorFilter = SelectFiltersByname("Authors", value);
                OnPropertyChanged(nameof(AuthorFilter));
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
                GenresFilter = SelectFiltersByname("Genres", value);
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
                LanguagesFilter = SelectFiltersByname("Languages", value);
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
                PublishersFilters = SelectFiltersByname("Publishers", value);
                OnPropertyChanged(nameof(PublishersFilters));
            }
        }


        // method for selecting only filters in substring
        private ObservableCollection<FilterOption> SelectFiltersByname(string table, string txt)
        {
            ObservableCollection <FilterOption> list = new ObservableCollection<FilterOption>(
                    db.SelectNameByTableName(table).Where(elem => elem.Name.ToLower().Contains(txt.ToLower()))
                                               .Select(elem => new FilterOption { Name = elem.Name }));

            return SetPropertyChange(list);
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

            // filling filters
            AuthorFilter = SelectFiltersByname("Authors", "");
            GenresFilter = SelectFiltersByname("Genres", "");
            LanguagesFilter = SelectFiltersByname("Languages", "");
            PublishersFilters = SelectFiltersByname("Publishers", "");
            

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

        private void FillItems(List<Book> list)
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
            BookCards = new ObservableCollection<Book>();
            List<Book> list = db.SelectAllBooks();

           FillItems(list); 
        }
        

        // Method for reseting filters
        private void DeleteFilters(object sender, RoutedEventArgs e)
        {
            DelFilters(AuthorFilter);
            DelFilters(GenresFilter);
            DelFilters(PublishersFilters);
            DelFilters(LanguagesFilter);
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
            var Author = DoFilter(AuthorFilter);
            var genre = DoFilter(GenresFilter);
            var languages = DoFilter(LanguagesFilter);
            var publishers = DoFilter(PublishersFilters);

            List<Book> list = db.SelectBooksByFilters(Author, genre, languages, publishers);

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


            List<Book> list = db.OrderBooks(column, way);
            FillItems(list);
        }

        // click method for button which start searching if substring in searchbar is in some book names or author names
        private void StartSearchBook(object sender, EventArgs e)
        {
            List<Book> list = db.SelectBooksWithSearch(searchBar.Text);
            FillItems(list);
        }

        // metod for opening form for adding book
        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            AddBookForm win = new AddBookForm();

			win.Closed += (s, eArgs) =>
			{
				SelectCards();
                OnPropertyChanged(nameof(BookCards));
			};

			win.ShowDialog();
		}

        // click method for redirect to AuthorWindow
        private void OpenAuthor(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorWindow();
        }
    }

    
}
