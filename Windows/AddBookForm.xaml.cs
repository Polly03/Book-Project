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

        public AddBookForm(Models.Func func, Book? book = null)
        {
            InitializeComponent();

            DataContext = this;

            Author = db.SelectNameByTableName("Authors");
            Genres = db.SelectNameByTableName("Genres");
            Languages = db.SelectNameByTableName("Languages");
            Publishers = db.SelectNameByTableName("Publishers");
            Types = db.SelectNameByTableName("BOOKTYPES");

            if (func == Func.Edit)
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

            AuthorBox.SelectedItem = Author.FirstOrDefault(a => a.Name == EditBook.Author);
            GenresBox.SelectedItem = Genres.FirstOrDefault(g => g.Name == EditBook.Genre);
            PublishersBox.SelectedItem = Publishers.FirstOrDefault(p => p.Name == EditBook.Publisher);
            LanguageBox.SelectedItem = Languages.FirstOrDefault(l => l.Name == EditBook.Langueage);
            TypesBox.SelectedItem = Types.FirstOrDefault(t => t.Name == EditBook.Type);

            NameBox.Text = EditBook.Name;
            ISBNBox.Text = EditBook.ISBN;
            EANBox.Text = EditBook.EAN;
            LengthBox.Text = EditBook.Length.ToString();
            RatingBox.Text = EditBook.Rating;
            DescriptionBox.Text = EditBook.Description;
            PhotoBox.Source = EditBook.Image;

        }

        private void ControlBeforeSaveEdit(object sender, RoutedEventArgs e)
        {
            if (!ControlData())
            {
                return;
            }

         

            byte[] image = BitMapToByte(PhotoBox.Source as BitmapImage);
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
            EditBook = db.SelectBook(bookName, Models.Size.Medium);

            this.Close();
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

            if (PhotoBox.Source == null) { MessageBox.Show("vyber fotku"); return false; }

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
            BitmapImage path = (BitmapImage)PhotoBox.Source;

            byte[]? photo = BitMapToByte(path);
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
                PhotoBox.Source = new BitmapImage(new Uri(filePath));
            }
        }

        private bool AreAllFieldsEmpty()
        {
            return AuthorBox.SelectedItem == null
                && GenresBox.SelectedItem == null
                && LanguageBox.SelectedItem == null
                && TypesBox.SelectedItem == null
                && PublishersBox.SelectedItem == null
                && string.IsNullOrWhiteSpace(NameBox.Text)
                && string.IsNullOrWhiteSpace(ISBNBox.Text)
                && string.IsNullOrWhiteSpace(EANBox.Text)
                && string.IsNullOrWhiteSpace(LengthBox.Text)
                && string.IsNullOrWhiteSpace(RatingBox.Text)
                && string.IsNullOrWhiteSpace(DescriptionBox.Text)
                && PhotoBox.Source == null;
        }

        public void Return(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsEmpty())
            {
                this.Close();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
               "Opravdu se chcete vrátit?\nVaše vyplněné pole budou ztraceny!",
               "Confirmation",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question
               );

                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            
            }
         
        }

        private void AddAuthorButton(object sender, RoutedEventArgs e)
        {
            AddAuthorForm win = new AddAuthorForm(Func.Add);

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
