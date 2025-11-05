using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabase.Models
{
    public class BooksData
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }

        public BooksData(string Title, string Author, string Genre)
        {
            this.Author = Author;
            this.Title = Title;
            this.Genre = Genre;
        }
    }
}
