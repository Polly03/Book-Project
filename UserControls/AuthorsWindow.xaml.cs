using BookDatabase.Models;
using BookDatabase.UserControls;
using BookDatabase.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


using System.Windows.Input;


namespace BookDatabase
{

    public partial class AuthorsWindow : UserControl
    {
        Database db = Database.Instance;

        ObservableProperties p = ObservableProperties.Instance;

        public AuthorsWindow()
        {
            InitializeComponent();

            DataContext = p;
            SelectCards();


            p.Countries = FillCheckBoxes("Countries");


            p.FilteredCountries = new ObservableCollection<FilterOption>(p.Countries);
   
            // filling properties

            p.Countries = SetPropertyChange(p.Countries);
 
            // adding event method when filter is checked or unchecked
        }

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
            p.MyItemsAuthors = new ObservableCollection<Authors>();
            List<Tuple<string, DateTime, string>> list = db.SelectAllAuthors();

            foreach (var item in list)
            {
                string? Date = item.Item2.ToString();
                p.MyItemsAuthors.Add(new Authors(item.Item1, Date, item.Item3));
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

            var countries = DoFilter(p.Countries);

          
            List<Tuple<string, DateTime, string>> list = db.SelectAuthorsByFilters(countries);
            foreach (var item in list)
            {


            }
                p.MyItemsAuthors.Clear();
                foreach (var elem in list)
                {
                    string txt = elem.Item2.ToString();
                    p.MyItemsAuthors.Add(new Authors(elem.Item1, txt , elem.Item3));
                }
        }

        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            string column = "";
            string way = "";
            if (s == "ABCasc") { column = "Authors.name"; way = "asc"; }
            if (s == "ABCdsc") { column = "Authors.name"; way = "desc"; }
            if (s == "Birthasc") { column = "Authors.DateOfBirth"; way = "asc"; }
            if (s == "Birthdsc") { column = "Authors.DateOfBirth"; way = "desc"; }


        // in work
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

        private void OrderMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void OpenBooks(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AddAuthor(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm("Author");
            win.ShowDialog();
        }
    }
}
