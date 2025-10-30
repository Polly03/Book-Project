using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookDatabase
{
 
    public partial class BooksWindow : UserControl, INotifyPropertyChanged

    {

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

        private ObservableCollection<CardData> MyItems { get; set; }

        public BooksWindow()
        {
            InitializeComponent();

            MyItems = new ObservableCollection<CardData>();
            DataContext = this;

            for (int i = 0; i < 5; i++)
            {
                MyItems.Add(new CardData { Width = 150, Height = 200 });
            }

            Authors = new ObservableCollection<FilterOption>{};
            Database db = new Database();
            List<string> listOfAuthors = db.SelectTableByName("Authors");
            foreach (string elem in listOfAuthors)
            {
                FilterOption option = new FilterOption();
                option.Name = elem;
                Authors.Add(option);
            }

            Genres = new ObservableCollection<FilterOption>{};
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyItems.Add(new CardData { Width = 150, Height = 200 });

            
        }


        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            ((ComboBox)item.Parent).Text = item.Content.ToString();
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

        private void test2_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }

    public class CardData
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public BitmapImage image { get; set; }

    }

    public class FilterOption : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
