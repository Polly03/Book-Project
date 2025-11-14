
namespace BookDatabase.Models
{
    public class Authors
    {
        // Model of Author Card

        public int Id { get; set; }
        public string? NameOf {  get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? AboutAuthor { get; set; }

        public Authors (string NameOf, string Date, string Country)
        {
            this.NameOf = NameOf;
            this.DateOfBirth = Date;
            this.Country = Country;
        }
    }
}
