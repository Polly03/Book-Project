using BookDatabase.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.WebRequestMethods;
using static BookDatabase.Models.BooksData;

namespace BookDatabase
{

    public partial class BooksWindow : UserControl, INotifyPropertyChanged

    {
        public ObservableCollection<BooksData> MyItems { get; set; }
        public ObservableCollection<FilterOption> Authors { get; set; }
        public ObservableCollection<FilterOption> Genres { get; set; }
        public ObservableCollection<FilterOption> Languages { get; set; }
        public ObservableCollection<FilterOption> Publishers { get; set; }

        private string _searchTextAuthor;
        private string _searchTextGenre;
        private string _searchTextPublisher;
        private string _searchTextLanguages;

        /* 
         * vlastnosti pro 
         * 1. interní deklaraci filtrovaných autorů (tedy textu v textBoxu), 
         * 2. veřejné nastavení autorů na tu interní se kteoru pracujeme (užviatel zadá "K" a interní vlastno s "_" se nastaví na veřejnou
         * 3. 
         */
        private ObservableCollection<FilterOption> _filteredAuthors;
        public ObservableCollection<FilterOption> FilteredAuthors
        {
            get => _filteredAuthors;
            set
            {
                _filteredAuthors = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextAuthor
        {
            get => _searchTextAuthor;
            set
            {
                _searchTextAuthor = value;
                OnPropertyChanged();
                ApplyFilterAuthors();
            }
        }

        /* stejné co u autorů ale pro žánry */
        private ObservableCollection<FilterOption> _filteredGenres;
        public ObservableCollection<FilterOption> FilteredGenres
        {
            get => _filteredGenres;
            set {
                _filteredGenres = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextGenre
        {
            get => _searchTextGenre;
            set { _searchTextGenre = value; OnPropertyChanged(); ApplyFilterGenres(); }
        }



        private ObservableCollection<FilterOption> _filteredLanguages;
        public ObservableCollection<FilterOption> FilteredLanguages
        {
            get => _filteredLanguages;
            set {
                _filteredLanguages = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<FilterOption> _filteredPublishers;
        public ObservableCollection<FilterOption> FilteredPublishers
        {
            get =>
                _filteredPublishers;
            set {
                _filteredPublishers = value;
                OnPropertyChanged(); }
        }


        public BooksWindow()
        {
            InitializeComponent();

            DataContext = this;
            Database db = new Database();

            MyItems = new ObservableCollection<BooksData>();
            List<Tuple<Byte[], string, string, string>> list = db.SelectAllBooks();  // photo, name, author, genre

            foreach (var item in list)
            {
                MyItems.Add(new BooksData(150, 200, item.Item2, item.Item3, item.Item4));

            }



            Authors = new ObservableCollection<FilterOption> { };

            List<string> listOfAuthors = db.SelectTableByName("Authors");
            foreach (string elem in listOfAuthors)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                Authors.Add(option);
            }

            Genres = new ObservableCollection<FilterOption> { };
            List<string> listOfGenres = db.SelectTableByName("Genres");
            foreach (string elem in listOfGenres)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                Genres.Add(option);
            }

            Languages = new ObservableCollection<FilterOption> { };
            List<string> listOfLanguages = db.SelectTableByName("Languages");
            foreach (string elem in listOfLanguages)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                Languages.Add(option);
            }

            Publishers = new ObservableCollection<FilterOption> { };
            List<string> listOfPublishers = db.SelectTableByName("Publishers");
            foreach (string elem in listOfPublishers)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                Publishers.Add(option);
            }

            FilteredAuthors = new ObservableCollection<FilterOption>(Authors);
            FilteredGenres = new ObservableCollection<FilterOption>(Genres);
            FilteredLanguages = new ObservableCollection<FilterOption>(Languages);
            FilteredPublishers = new ObservableCollection<FilterOption>(Publishers);

            foreach (var author in Authors)
            {
                author.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }

            foreach (var genre in Genres)
            {
                genre.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }

            foreach (var language in Languages)
            {
                language.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }

            foreach (var publisher in Publishers)
            {
                publisher.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }

        }

        private void ApplyFilter()
        {
            var authors = GetSelectedAuthors().Count == 0 ? "" : 
                string.Join(",", GetSelectedAuthors().Select(a => $"'{a.Replace("'", "''")}'"));

            var genres = GetSelectedGenres().Count == 0 ? "" : 
                string.Join(",",  GetSelectedGenres().Select(a => $"'{a.Replace("'", "''")}'"));

            var languages = GetSelectedLanguages().Count == 0 ? "" : 
                string.Join(",", GetSelectedLanguages().Select(a => $"'{a.Replace("'", "''")}'"));

            var publishers = GetSelectedPublishers().Count == 0 ? "" : 
                string.Join(",", GetSelectedPublishers().Select(a => $"'{a.Replace("'", "''")}'"));

            Database db = new Database();
            List<Tuple<byte[], string, string, string>> list = db.SelectBooksByFilters(authors, genres, languages, publishers);

            MyItems.Clear();
            foreach (var elem in list)
            {
                MyItems.Add(new BooksData(150, 200, elem.Item2, elem.Item3, elem.Item4));
            }
        }

        private List<string> GetSelectedAuthors()
        {
            return Authors.Where(a => a.IsSelected).Select(a => a.Name).ToList();
        }

        private List<string> GetSelectedGenres()
        {
            return Genres.Where(a => a.IsSelected).Select(a => a.Name).ToList();
        }

        private List<string> GetSelectedLanguages()
        {
            return Languages.Where(a => a.IsSelected).Select(a => a.Name).ToList();
        }

        private List<string> GetSelectedPublishers()
        {
            return Publishers.Where(a => a.IsSelected).Select(a => a.Name).ToList();
        }




        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            Database db = new Database();
            string column = "";
            string way = "";
            if (s == "ABCasc") { column = "books.name"; way = "asc"; }
            if (s == "ABCdsc") { column = "books.name"; way = "desc"; }
            if (s == "LENasc") { column = "books.length"; way = "asc"; }
            if (s == "LENdsc") { column = "books.length"; way = "desc"; }

            
            List<Tuple<byte[], string, string, string>> list = db.OrderBooks(column, way);
            MyItems.Clear();
            foreach (var elem in list)
            {
                MyItems.Add(new BooksData(150, 200, elem.Item2, elem.Item3, elem.Item4));
            }
        }

        private void StartSearchBook(object sender, EventArgs e)
        {
            Database db = new Database();
            List <Tuple<Byte[], string, string, string>> list = db.SelectBooksWithSearch(searchBar.Text);
            MyItems.Clear();
            foreach (var elem in list)
            {
                MyItems.Add(new BooksData(150, 200, elem.Item2, elem.Item3, elem.Item4));
            }

        }


        private void ApplyFilterAuthors()
        {
            if (string.IsNullOrWhiteSpace(SearchTextAuthor))
            {
                FilteredAuthors = new ObservableCollection<FilterOption>(Authors);
            }

            else
            {
                FilteredAuthors = new ObservableCollection<FilterOption>
                                  (Authors.Where(a => a.Name.ToLower().Contains(SearchTextAuthor.ToLower())));
            }

        }




        public string SearchTextLanguages
        {
            get => _searchTextLanguages;
            set { _searchTextLanguages = value; OnPropertyChanged(); ApplyFilterLanguages(); }
        }


        public string SearchTextPublisher
        {
            get => _searchTextPublisher;
            set { _searchTextPublisher = value; OnPropertyChanged(); ApplyFilterPublishers(); }
        }




        private void ApplyFilterGenres()
        {
            if (string.IsNullOrWhiteSpace(SearchTextGenre))
                FilteredGenres = new ObservableCollection<FilterOption>(Genres);
            else
                FilteredGenres = new ObservableCollection<FilterOption>(
                    Genres.Where(g => g.Name.ToLower().Contains(SearchTextGenre.ToLower()))
                );
        }

        private void ApplyFilterLanguages()
        {
            if (string.IsNullOrWhiteSpace(SearchTextLanguages))
                FilteredLanguages = new ObservableCollection<FilterOption>(Languages);
            else
                FilteredLanguages = new ObservableCollection<FilterOption>(
                    Languages.Where(l => l.Name.ToLower().Contains(SearchTextLanguages.ToLower()))
                );
        }

        private void ApplyFilterPublishers()
        {
            if (string.IsNullOrWhiteSpace(SearchTextPublisher))
                FilteredPublishers = new ObservableCollection<FilterOption>(Publishers);
            else
                FilteredPublishers = new ObservableCollection<FilterOption>(
                    Publishers.Where(p => p.Name.ToLower().Contains(SearchTextPublisher.ToLower()))
                );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AddBookForm();
        }

        private void OpenAuthors(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorsWindow();
        }
    }

    
}
