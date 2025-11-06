using BookDatabase.Models;
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

    public partial class BooksWindow : UserControl, INotifyPropertyChanged

    {
        public ObservableCollection<BooksData> MyItems { get; set; }
        public ObservableCollection<FilterOption> Authors { get; set; }
        public ObservableCollection<FilterOption> Genres { get; set; }
        public ObservableCollection<FilterOption> Languages { get; set; }
        public ObservableCollection<FilterOption> Publishers { get; set; }

        private string _searchTextAuthor = string.Empty;
        private string _searchTextGenre = string.Empty;
        private string _searchTextPublisher = string.Empty;
        private string _searchTextLanguages = string.Empty;

        /* 
         * vlastnosti pro 
         * 1. interní deklaraci filtrovaných autorů (tedy textu v textBoxu), 
         * 2. veřejné nastavení autorů na tu interní se kteoru pracujeme (užviatel zadá "K" a interní vlastno s "_" se nastaví na veřejnou
         * 3. nastavení metody applyfilters pro filtry, aby při změně vyhledávání filtru se zobrazily jen ty, co mají v názvu podstring
         */
        private ObservableCollection<FilterOption> _filteredAuthors = new ObservableCollection<FilterOption>();
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
                FilteredAuthors = new ObservableCollection<FilterOption>(
                    Authors.Where(p => p.Name!.ToLower().Contains(SearchTextAuthor.ToLower())));
            }
        }

        /* stejné co u autorů ale pro žánry a všecny ostatní filtry */

        private ObservableCollection<FilterOption> _filteredGenres = new ObservableCollection<FilterOption>();
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
            set {
                _searchTextGenre = value;
                OnPropertyChanged();
                FilteredGenres = new ObservableCollection<FilterOption>(
                    Genres.Where(p => p.Name!.ToLower().Contains(SearchTextGenre.ToLower())));
            }
        }



        private ObservableCollection<FilterOption> _filteredLanguages = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredLanguages
        {
            get => _filteredLanguages;
            set {
                _filteredLanguages = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextLanguages
        {
            get => _searchTextLanguages;
            set {
                _searchTextLanguages = value;
                OnPropertyChanged();
                FilteredLanguages = new ObservableCollection<FilterOption>(
                    Languages.Where(p => p.Name!.ToLower().Contains(SearchTextLanguages.ToLower())));
            }
        }

        private ObservableCollection<FilterOption> _filteredPublishers = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredPublishers
        {
            get => _filteredPublishers;
            set {
                _filteredPublishers = value;
                OnPropertyChanged();
            }
        }

        public string SearchTextPublisher
        {
            get => _searchTextPublisher;
            set {
                _searchTextPublisher = value;
                OnPropertyChanged();
                FilteredPublishers = new ObservableCollection<FilterOption>(
                                    Publishers.Where(p => p.Name!.ToLower().Contains(SearchTextPublisher.ToLower())));
            }
        }

        public BooksWindow()
        {
            // základní zobrazení všech knih
            InitializeComponent();

            DataContext = this;
            Database db = new Database();
            SelectCards();

            // přidání autorů, žánrů atd do filtrů jako klikací checkbox

            Authors = FillCheckBoxes("Authors");
            Genres = FillCheckBoxes("Genres");
            Languages = FillCheckBoxes("Languages");
            Publishers = FillCheckBoxes("Publishers");

            FilteredAuthors = new ObservableCollection<FilterOption>(Authors);
            FilteredGenres = new ObservableCollection<FilterOption>(Genres);
            FilteredLanguages = new ObservableCollection<FilterOption>(Languages);
            FilteredPublishers = new ObservableCollection<FilterOption>(Publishers);

            // každý filter má klikací metodu, která po zaškrtnutí filtru zobrazí knihy s daným filtrem

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

        private ObservableCollection<FilterOption> FillCheckBoxes(string txt)
        {
            ObservableCollection<FilterOption> filters = new ObservableCollection<FilterOption>();
            Database db = new Database();
            List<string> list = db.SelectTableByName(txt);
            foreach (string elem in list)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                filters.Add(option);
            }

            return filters;
        }

        private void SelectCards()
        {

            Database db = new Database();
            MyItems = new ObservableCollection<BooksData>();

            List<Tuple<byte[], string, string, string>> list = db.SelectAllBooks();

           

            foreach (var item in list)
            {
         
                MyItems.Add(new BooksData(GetBM(item.Item1), item.Item2, item.Item3, item.Item4));

            }
        }

        private BitmapImage GetBM(byte[] data)
        {
            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(data))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }
            return bitmap;
        }

        // metoda pro tlačítko na smazání všech filtrů
        private void DeleteFilters(object sender, RoutedEventArgs e)
        {

            foreach (var author in Authors)
            {
                author.IsSelected = false;
            }
            foreach (var genre in Genres)
            {
                genre.IsSelected = false;
            }
            foreach (var language in Languages)
            {
                language.IsSelected = false;
            }
            foreach (var publisher in Publishers)
            {
                publisher.IsSelected = false;
            }
        }

        // při zaškrtnutí dalšího filtru se znovu zavolá SQL procedura a vrátí se knihy odpvídajícím prvkům
        // zároven jsou stringu přidány jednoduché uvozovky aby se mohl reprezentovat jako list v SQL
        private void ApplyFilter()
        {
            var authors = DoFilter(Authors);

            var genres = DoFilter(Genres);

            var languages = DoFilter(Languages);

            var publishers = DoFilter(Publishers);

            Database db = new Database();
            List<Tuple<byte[], string, string, string>> list = db.SelectBooksByFilters(authors, genres, languages, publishers);

            MyItems.Clear();
            foreach (var elem in list)
            {
                MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }
        }

        private string DoFilter(ObservableCollection<FilterOption> list)
        {
            return GetSelected(list).Count == 0 ? "" :
                string.Join(",", GetSelected(list).Select(a => $"'{a.Replace("'", "''")}'"));
        }

        private List<string> GetSelected(ObservableCollection<FilterOption> list)
        {
            return list.Where(a => a.IsSelected).Select(a => a.Name).ToList() as List<string>;
        }


        // metoda pro volání sql procedury která vrátí knihy ORDER BY určitého elementu
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
                MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }
        }

        // metoda, která se spustí tlačítkem které vyhledá a zobrazí knihy obsahující substring v názvu nebo v autorově jméně
        private void StartSearchBook(object sender, EventArgs e)
        {
            Database db = new Database();
            List<Tuple<Byte[], string, string, string>> list = db.SelectBooksWithSearch(searchBar.Text);
            MyItems.Clear();
            foreach (var elem in list)
            {
                MyItems.Add(new BooksData(GetBM(elem.Item1), elem.Item2, elem.Item3, elem.Item4));
            }

        }

        // eventy pro poslouchání změny prvků
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        // metody buttonů pro otevření dalších oken
        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            AddBookForm win = new AddBookForm();
            win.ShowDialog();
        }

        private void OpenAuthors(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new AuthorsWindow();
        }
    }

    
}
