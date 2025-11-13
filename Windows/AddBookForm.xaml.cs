using BookDatabase.Models;
using BookDatabase.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookDatabase
{

    public partial class AddBookForm : Window
    {
        public List<GeneralModel> Authors { get; set; }
        public List<GeneralModel> Genres { get; set; }
        public List<GeneralModel> Languages { get; set; }
        public List<GeneralModel> Publishers { get; set; }
        public List<GeneralModel> Types { get; set; }

        Database db = Database.Instance;

        public AddBookForm()
        {
            InitializeComponent();

            DataContext = this;

            Authors = GetProperties("Authors");
            Genres = GetProperties("Genres");
            Languages = GetProperties("Languages");
            Publishers = GetProperties("Publishers");
            Types = GetProperties("BOOKTYPES");
        }

        private List<GeneralModel> GetProperties(string name)
        {
            List<GeneralModel> list = new List<GeneralModel>();
            List<GeneralModel> lists = db.SelectNameByTableName(name);
            foreach (GeneralModel item in lists)
            {
                list.Add(item);
            }

            return list;

        }

        private void Length_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
          
            Regex regex = new Regex("^[0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }



        private void ControlBeforeSave(object sender, RoutedEventArgs e)
        {
            if (AuthorsBox.SelectedItem == null) { MessageBox.Show("vyber autora"); return; }
            if (GenresBox.SelectedItem == null) { MessageBox.Show("vyber Žánr");    return; }
            if (LanguageBox.SelectedItem == null) { MessageBox.Show("vyber jazyk"); return; }
            if (TypesBox.SelectedItem == null) { MessageBox.Show("vyber typ knihy"); return; }
            if (PublishersBox.SelectedItem == null) { MessageBox.Show("vyber vydavatele"); return; }

            if (String.IsNullOrWhiteSpace(NameBox.Text)) { MessageBox.Show("vypln název knihy"); return; }
            if (String.IsNullOrWhiteSpace(ISBNBox.Text)) { MessageBox.Show("vypln ISBN"); return; }
            if (String.IsNullOrWhiteSpace(EANBox.Text)) { MessageBox.Show("vypln EAN"); return; }
            if (String.IsNullOrWhiteSpace(LengthBox.Text)) { MessageBox.Show("vypln délku knihy"); return; }

            if (String.IsNullOrWhiteSpace(RatingBox.Text)) { MessageBox.Show("vypln zhodnocení knihy"); return; }
            if (String.IsNullOrWhiteSpace(DescriptionBox.Text)) { MessageBox.Show("vypln popis knihy"); return; }

            if (String.IsNullOrWhiteSpace(PhotoBox.Text)) { MessageBox.Show("vyber fotku"); return; }

            if (ISBNBox.Text.Length < 13)
            {
                MessageBox.Show("ISBN musí mít přesně 13 znaků");
             }

            if (EANBox.Text.Length < 13)
            {
                MessageBox.Show("EAN musí mít přesně 13 znaků");
            }

            InputData();

            this.Close();
        }

        private void InputData()
        {
            string path = PhotoBox.Text;


            byte[]? photo = GetPhotoByPath(path);
            string? typeOfBook = ((GeneralModel)TypesBox.SelectedItem).Name;
            short lengthOfBook = short.Parse(LengthBox.Text);
            string? ean = EANBox.Text;
            string? isbn = ISBNBox.Text;
            string? publisher = ((GeneralModel)PublishersBox.SelectedItem).Name;
            string? language = ((GeneralModel)LanguageBox.SelectedItem).Name;
            string? rating = RatingBox.Text;
            string? description = DescriptionBox.Text;
            string? genre = ((GeneralModel)GenresBox.SelectedItem).Name;
            string? bookName = NameBox.Text;
            string? authorName = ((GeneralModel)AuthorsBox.SelectedItem).Name;

            db.InsertBookOldAuthor(
                photo,
                typeOfBook,
                lengthOfBook,
                ean,
                isbn,
                publisher,
                language,
                rating,
                description,
                genre,
                bookName,
                authorName
            );

        }

        private byte[] GetPhotoByPath(string path)
        {
            // Načti obrázek s omezeným rozlišením už při dekódování
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;

            bitmap.DecodePixelWidth = 125;
            bitmap.DecodePixelHeight = 150;                           

            bitmap.EndInit();
            bitmap.Freeze();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 85; 
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }


        private void ChoosePhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.bmp|All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                MessageBox.Show(filePath);
                PhotoBox.Text = filePath;
            }
        }

        public void Return(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddAuthorButton(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm("Book");
            win.ShowDialog();
            win.AuthorAdded += ResetAuthors;

        }
        private void ResetAuthors()
        {
            Authors.Clear();
            Authors = GetProperties("Authors");
        }
    }
}
