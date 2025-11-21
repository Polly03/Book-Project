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

    public partial class AuthorWindow : UserControl, INotifyPropertyChanged
    {
        Database db = Database.Instace;


        // collection for Author Cards
        public ObservableCollection<Author> AuthorCards { get; set; }


        // properties and collections for filters for Author Cards
        public ObservableCollection<FilterOption> CountryFilters { get; set; }
        private string fSearchCountryFilter = string.Empty;
        public string SearchCountryFilter
        {
            get => fSearchCountryFilter;
            set
            {
                fSearchCountryFilter = value;
                CountryFilters = SelectFiltersByname("Countries", value);
                OnPropertyChanged(nameof(CountryFilters));
            }
        }

        // selecting Authors by Filter
        private ObservableCollection<FilterOption> SelectFiltersByname(string table, string txt)
        {
            ObservableCollection<FilterOption> list = new ObservableCollection<FilterOption>(
                    db.SelectNameByTableName(table).Where(elem => elem.Name.ToLower().Contains(txt.ToLower()))
                                               .Select(elem => new FilterOption { Name = elem.Name }));

            return SetPropertyChange(list);
        }


        // event listener for automatic change
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public AuthorWindow()
        {
            InitializeComponent();

            DataContext = this;
            SelectCards();

            CountryFilters = SelectFiltersByname("Countries", "");
        }


        // start method for showing all books or refresh books
        private void SelectCards()
        {
            AuthorCards = new ObservableCollection<Author>();
            List<Author> list = db.SelectAllAuthor();

                foreach (var item in list)
                {
                    AuthorCards.Add(item);
                }
        }

        // method for setting elements of list method to start when property of element change
        private ObservableCollection<FilterOption> SetPropertyChange(ObservableCollection<FilterOption> list)
        {
            foreach (var elem in list)
            {
                elem.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterOption.IsSelected)) { ApplyFilter(); } };
            }
            return list;
        }

        // 
        private void ApplyFilter()
        {

            var countries = DoFilter(CountryFilters);
            List<Author> list = db.SelectAuthorByFilters(countries);
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

            List<Author> Author = db.OrderAuthor(column, way);
            FillItems(Author);
        }

        private void FillItems(List<Author> list)
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
            AddAuthorForm win = new AddAuthorForm(Models.Func.Add);
            win.Closed += (s, eArgs) =>
            {
                SelectCards();
                OnPropertyChanged(nameof(AuthorCards));
            };

            win.ShowDialog();
        }

        private void StartSearchAuthor(object sender, RoutedEventArgs e)
        {
            List<Author> Author = db.SelectAuthorWithSearch(SearchBar.Text);
            FillItems(Author);
        }
    }
}
