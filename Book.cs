using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookDatabase
{
    internal class Book
    {
        [Key]
        public int ID {  get; set; }
        private int AuthorID {  get; set; }
        [MaxLength(64)]
        private string Name { get; set; }
        private int GenreID {  get; set; }
        private string Description { get; set; }
        private string Rating {  get; set; }
        private int Publishing_HouseID { get; set; }
        [MaxLength(13)]
        private string ISBN { get; set; }
        [MaxLength(13)]
        private string EAN { get;set; }
        private short Length {  get; set; }
        private int TypeID { get; set; }

        public Book (int iD, int authorID, string name, int genreID, string description, string rating, int publishing_HouseID, string isbn, string ean, short length, int typeID)
        {
            ID = iD;
            AuthorID = authorID;
            Name = name;
            GenreID = genreID;
            Description = description;
            Rating = rating;
            Publishing_HouseID = publishing_HouseID;
            ISBN = isbn;
            EAN = ean;
            Length = length;
            TypeID = typeID;
        }
    }
}
