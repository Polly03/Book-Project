using BookDatabase.Models;
using BookDatabase.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


using System.Windows.Input;


namespace BookDatabase
{

    public partial class AuthorsWindow : UserControl, INotifyPropertyChanged
    {
        Database db = Database.Instance;


        // collection for Author Cards
        public ObservableCollection<Authors> AuthorCards { get; set; }


        // properties and collections for filters for Author Cards
        public ObservableCollection<FilterOption> FilteredCountries { get; set; }
        private string fSearchTextCountries = string.Empty;
        public string SearchTextCountries
        {
            get => fSearchTextCountries;
            set
            {
                fSearchTextCountries = value;
                FilteredCountries.Clear();
                FilteredCountries = filter("Countries", value);
                OnPropertyChanged(nameof(FilteredCountries));
            }
        }

        // filtering filters
        private ObservableCollection<FilterOption> filter(string table, string txt)
        {
            return new ObservableCollection<FilterOption>(
                db.SelectNameByTableName(table).Where(elem => elem.Name.ToLower().Contains(txt.ToLower()))
                                               .Select(elem => new FilterOption { Name = elem.Name })
            );
        }

        // event
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public AuthorsWindow()
        {
            InitializeComponent();

            DataContext = this;
            SelectCards();


            FilteredCountries = FillCheckBoxes("Countries");


            FilteredCountries = new ObservableCollection<FilterOption>(FilteredCountries);
   
            // filling properties

            FilteredCountries = SetPropertyChange(FilteredCountries);
 
            // adding event method when filter is checked or unchecked
        }

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

        // start method for showing all books or refresh books
        private void SelectCards()
        {
            AuthorCards = new ObservableCollection<Authors>();
            List<Authors> list = db.SelectAllAuthors();

            foreach (var item in list)
            {
                AuthorCards.Add(item);
            }
        }

        private ObservableCollection<FilterOption> SetPropertyChange(ObservableCollection<FilterOption> list)
        {
            foreach (var elem in list)
            {
                elem.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }
            return list;
        }

        private void ApplyFilter()
        {

            var countries = DoFilter(FilteredCountries);

          
            List<Authors> list = db.SelectAuthorsByFilters(countries);
            foreach (var item in list)
            {


            }
                FillItems(list);
        }

        private void OrderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            string column = "";
            string way = "";
            if (s == "ABCasc") { column = "Authors.Name"; way = "asc";  }
            else if (s == "ABCdsc") { column = "Authors.Name"; way = "desc";  }
            else if (s == "Birthasc") { column = "Authors.DateOfBirth"; way = "asc";  }
            else if (s == "Birthdsc") { column = "Authors.DateOfBirth"; way = "desc";  }

            List<Authors> authors = db.OrderAuthors(column, way);
            FillItems(authors);
        }

        private void FillItems(List<Authors> list)
        {
            AuthorCards.Clear();
            foreach (var elem in list)
            {
                AuthorCards.Add(elem);
            }
        }

        private string DoFilter(ObservableCollection<FilterOption> list)
        {
            List<string> selected = GetSelected(list);
            return selected.Count == 0 ? "" :
                string.Join(",", selected.Select(a => $"'{a.Replace("'", "''")}'"));
        }

        private List<string> GetSelected(ObservableCollection<FilterOption> list)
        {
            return list.Where(a => a.IsSelected).Select(a => a.Name).ToList() as List<string>;
        }



        private void Return(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        private void OpenBooks(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }
        private void AddAuthor(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm("Author");
            win.Closed += (s, eArgs) =>
            {

                SelectCards();
            };

            win.ShowDialog();
        }

        private void StartSearchAuthor(object sender, RoutedEventArgs e)
        {
            List<Authors> authors = db.SelectAuthorWithSearch(SearchBar.Text);
            FillItems(authors);
        }
    }
}
