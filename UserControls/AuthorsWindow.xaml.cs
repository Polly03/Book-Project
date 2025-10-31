﻿using BookDatabase.Models;
using BookDatabase.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;


namespace BookDatabase
{
    /// <summary>
    /// Interaction logic for AuthorsWindow.xaml
    /// </summary>
    public partial class AuthorsWindow : UserControl
    {

        public ObservableCollection<Authors> MyItemsAuthors { get; set; }
        public ObservableCollection<FilterOption> Countries { get; set; }


        private string _searchTextCountry;


        private ObservableCollection<FilterOption> _filteredCountries;
        public ObservableCollection<FilterOption> FilteredCountries
        {
            get => _filteredCountries;
            set
            {
                _filteredCountries = value;
                OnPropertyChanged();
            }
        }
        public string SearchTextAuthor
        {
            get => _searchTextCountry;
            set
            {
                _searchTextCountry = value;
                OnPropertyChanged();
                ApplyFilterCountries();
            }
        }

        private void ApplyFilterCountries()
        {
            if (string.IsNullOrWhiteSpace(SearchTextAuthor))
            {
                FilteredCountries = new ObservableCollection<FilterOption>(Countries);
            }

            else
            {
                FilteredCountries = new ObservableCollection<FilterOption>
                                  (Countries.Where(a => a.Name.ToLower().Contains(SearchTextAuthor.ToLower())));
            }

        }



        public AuthorsWindow()
        {
            InitializeComponent();

            DataContext = this;
            Database db = new Database();

            MyItemsAuthors = new ObservableCollection<Authors>();

            List<Tuple<string, string, string>> list = db.SelectAllAuthors();

            foreach (var item in list)
            {
                MyItemsAuthors.Add(new Authors(150, 200, item.Item1, item.Item2, item.Item3));
            }

        }

        private void Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ComboBoxItem)sender);
            var s = item.Name;
            ((ComboBox)item.Parent).Text = item.Content.ToString();
        }

        private void AddAuthor(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm("Authors");
            win.ShowDialog();
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void OpenBooks(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Main.Content = new BooksWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
