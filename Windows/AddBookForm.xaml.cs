using BookDatabase.Models;
using BookDatabase.Windows;
using Microsoft.Win32;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace BookDatabase
{

    public partial class AddBookForm : Window, INotifyPropertyChanged
    {
        public List<GeneralModel> Author { get; set; }
        public List<GeneralModel> Genres { get; set; }
        public List<GeneralModel> Languages { get; set; }
        public List<GeneralModel> Publishers { get; set; }
        public List<GeneralModel> Types { get; set; }

        public Book? EditBook { get; set; }

        Database db = Database.Instace;

        public AddBookForm(string func = "add", Book? book = null)
        {
            InitializeComponent();

            DataContext = this;

            Author = db.SelectNameByTableName("Authors");
            Genres = db.SelectNameByTableName("Genres");
            Languages = db.SelectNameByTableName("Languages");
            Publishers = db.SelectNameByTableName("Publishers");
            Types = db.SelectNameByTableName("BOOKTYPES");

            if (func == "edit")
            {
                EditBook = book!;
                this.Loaded += AddFormToEditForm;
            }
        }

        private void AddFormToEditForm(object sender, RoutedEventArgs e)
        {
            AddEditButton.Content = "EDIT";
            AddEditButton.Click -= ControlBeforeSave;
            AddEditButton.Click += ControlBeforeSaveEdit;

            AuthorBox.SelectedIndex = SelectIndeOfBox(Author, EditBook!.Author);
            GenresBox.SelectedIndex = SelectIndeOfBox(Genres, EditBook.Genre);
            PublishersBox.SelectedIndex = SelectIndeOfBox(Publishers, EditBook.Publisher);
            LanguageBox.SelectedIndex = SelectIndeOfBox(Languages, EditBook.Langueage);
            TypesBox.SelectedIndex = SelectIndeOfBox(Types, EditBook.Type);

            NameBox.Text = EditBook.Name;
            ISBNBox.Text = EditBook.ISBN;
            EANBox.Text = EditBook.EAN;
            LengthBox.Text = EditBook.Length.ToString();
            RatingBox.Text = EditBook.Rating;
            DescriptionBox.Text = EditBook.Description;
            PhotoBox.Text = "OLD";

        }

        private void ControlBeforeSaveEdit(object sender, RoutedEventArgs e)
        {
            if (!ControlData())
            {
                return;
            }

            byte[] image;
            if (PhotoBox.Text == "OLD")
            {
                image = BitMapToByte(EditBook!.Image!);
            }
            else
            {
                image = GetPhotoByPath(PhotoBox.Text);
            }

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
            string? authorName = ((GeneralModel)AuthorBox.SelectedItem).Name;

            db.UpdateBook(
                EditBook!.Id, 
                image, 
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
                authorName);
            EditBook = db.SelectBook(bookName);

            this.Close();
        }


        private int SelectIndeOfBox(List<GeneralModel> list, string? value)
        {
            foreach (GeneralModel model in list)
            {
                if (model.Name == value)
                {
                    return list.IndexOf(model);
                }
            }
            return 0;
        }

        private void Length_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
          
            Regex regex = new Regex("^[0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private bool ControlData()
        {
            if (AuthorBox.SelectedItem == null) { MessageBox.Show("vyber autora"); return false; }
            if (GenresBox.SelectedItem == null) { MessageBox.Show("vyber Žánr"); return false; ; }
            if (LanguageBox.SelectedItem == null) { MessageBox.Show("vyber jazyk"); return false; }
            if (TypesBox.SelectedItem == null) { MessageBox.Show("vyber typ knihy"); return false; }
            if (PublishersBox.SelectedItem == null) { MessageBox.Show("vyber vydavatele"); return false; }

            if (String.IsNullOrWhiteSpace(NameBox.Text)) { MessageBox.Show("vypln název knihy"); return false; }
            if (String.IsNullOrWhiteSpace(ISBNBox.Text)) { MessageBox.Show("vypln ISBN"); return false; }
            if (String.IsNullOrWhiteSpace(EANBox.Text)) { MessageBox.Show("vypln EAN"); return false; }
            if (String.IsNullOrWhiteSpace(LengthBox.Text)) { MessageBox.Show("vypln délku knihy"); return false; }

            if (String.IsNullOrWhiteSpace(RatingBox.Text)) { MessageBox.Show("vypln zhodnocení knihy"); return false; }
            if (String.IsNullOrWhiteSpace(DescriptionBox.Text)) { MessageBox.Show("vypln popis knihy"); return false; }

            if (String.IsNullOrWhiteSpace(PhotoBox.Text)) { MessageBox.Show("vyber fotku"); return false; }

            if (ISBNBox.Text.Length < 13)
            {
                MessageBox.Show("ISBN musí mít přesně 13 znaků");
                return false;
            }

            if (EANBox.Text.Length < 13)
            {
                MessageBox.Show("EAN musí mít přesně 13 znaků");
                return false;
            }

            return true;
        }


        private void ControlBeforeSave(object sender, RoutedEventArgs e)
        {

            if (ControlData()){

                InputData();

                this.Close();
            }
            else
            {
                return;
            }

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
            string? authorName = ((GeneralModel)AuthorBox.SelectedItem).Name;

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
       
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;

            bitmap.DecodePixelWidth = 520;
            bitmap.DecodePixelHeight = 800;                           

            bitmap.EndInit();
            bitmap.Freeze();

            return BitMapToByte(bitmap);
        }

        private byte[] BitMapToByte (BitmapImage image)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

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

            win.Closed += (s, eArgs) =>
            {

                Author = db.SelectNameByTableName("Authors");
                OnPropertyChanged(nameof(Author));
            };

            win.ShowDialog();

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
