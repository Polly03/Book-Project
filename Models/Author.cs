
namespace BookDatabase.Models
{
    public class Author
    {
        // Model of Author Card

        public int Id { get; set; }
        public string Name {  get; set; }  
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? AboutAuthor { get; set; }

        public Author (string Name, string Date, string Country)
        {
            this.Name = Name;
            this.DateOfBirth = Date;
            this.Country = Country;
        }
    }
}
