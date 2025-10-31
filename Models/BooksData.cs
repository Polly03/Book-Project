using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabase.Models
{
    public class BooksData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }

        public BooksData(int Width, int Height, string Title, string Author, string Genre)
        {
            this.Author = Author;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;
            this.Genre = Genre;
        }
    }
}
