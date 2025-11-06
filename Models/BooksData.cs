using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookDatabase.Models
{
    public class BooksData
    {
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
