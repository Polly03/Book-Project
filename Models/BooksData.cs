using System.Windows.Media.Imaging;

namespace BookDatabase.Models
{
    public class BooksData
    {
        // Model of Book Card
        public BitmapImage ImageSource {  get; set; } 
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }

        public BooksData(BitmapImage image, string Title, string Author, string Genre)
        {
            this.ImageSource = image;
            this.Author = Author;
            this.Title = Title;
            this.Genre = Genre;
        }
    }
}
