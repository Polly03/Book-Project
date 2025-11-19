using System.Windows.Media.Imaging;

namespace BookDatabase.Models
{
    // model for 1 book
    public class Book  //JR: z jakeho duvodu neni tato trida proste pouzita i pro ty karticky (kde pouzivate BookData? )
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Type { get; set; }
        public string? Langueage { get; set; }

        public short Length { get; set; }
        public string? Publisher { get; set; }
        public string? Description { get; set; }
        public string? EAN { get; set; }
        public string? ISBN { get; set; }
        public string? Rating { get; set; }

        public BitmapImage? Image { get; set; }

        public Book()
        {

        }
    }
}
