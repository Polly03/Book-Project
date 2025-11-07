using BookDatabase.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;


namespace BookDatabase.UserControls
{
    public sealed class ObservableProperties: INotifyPropertyChanged
    {
        private static ObservableProperties instance = null;

        public static ObservableProperties Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObservableProperties();
                }
                return instance;
            }
        }

        private ObservableProperties() { }

        public ObservableCollection<BooksData> MyItems { get; set; }
        public ObservableCollection<Authors> MyItemsAuthors { get; set; }
        public ObservableCollection<FilterOption> Authors { get; set; }
        public ObservableCollection<FilterOption> Genres { get; set; }
        public ObservableCollection<FilterOption> Languages { get; set; }
        public ObservableCollection<FilterOption> Publishers { get; set; }

        public ObservableCollection<FilterOption> Countries { get; set; }

        private string _searchTextAuthor = string.Empty;
        private string _searchTextGenre = string.Empty;
        private string _searchTextPublisher = string.Empty;
        private string _searchTextLanguages = string.Empty;
        private string _searchTextCountries = string.Empty;

        /*
         All properties for BooksWIndow And AuthorsWindow
         
         */

        private ObservableCollection<FilterOption> _filteredCountries = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredCountries
        {
            get => _filteredCountries;
            set
            {
                _filteredCountries = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextCountries
        {
            get => _searchTextCountries;
            set
            {
                _searchTextCountries = value;
                OnPropertyChanged();
                FilteredCountries = filter(Countries, SearchTextCountries);
            }
        }

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
                FilteredAuthors = filter(Authors, SearchTextAuthor);
            }
        }

        /* stejné co u autorů ale pro žánry a všecny ostatní filtry */

        private ObservableCollection<FilterOption> _filteredGenres = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredGenres
        {
            get => _filteredGenres;
            set
            {
                _filteredGenres = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextGenre
        {
            get => _searchTextGenre;
            set
            {
                _searchTextGenre = value;
                OnPropertyChanged();
                FilteredGenres = filter(Publishers, SearchTextPublisher);
            }
        }



        private ObservableCollection<FilterOption> _filteredLanguages = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredLanguages
        {
            get => _filteredLanguages;
            set
            {
                _filteredLanguages = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextLanguages
        {
            get => _searchTextLanguages;
            set
            {
                _searchTextLanguages = value;
                OnPropertyChanged();
                FilteredLanguages = filter(Languages, SearchTextLanguages);
            }
        }

        private ObservableCollection<FilterOption> _filteredPublishers = new ObservableCollection<FilterOption>();
        public ObservableCollection<FilterOption> FilteredPublishers
        {
            get => _filteredPublishers;
            set
            {
                _filteredPublishers = value;
                OnPropertyChanged();
            }
        }

        public string SearchTextPublisher
        {
            get => _searchTextPublisher;
            set
            {
                _searchTextPublisher = value;
                OnPropertyChanged();
                FilteredPublishers = filter(Publishers, SearchTextPublisher);
            }
        }

        private ObservableCollection<FilterOption> filter(ObservableCollection<FilterOption> list, string txt)
        {
            return new ObservableCollection<FilterOption>(list.Where(p => p.Name!.ToLower().Contains(txt.ToLower())));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
