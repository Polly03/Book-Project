using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabase.Models
{
    public class Authors
    {
        // Model of Author Card
        public string NameOf {  get; set; }
        public string DateOfBirth { get; set; }
        public string Country { get; set; }

        public Authors (string NameOf, string Date, string Country)
        {
            this.NameOf = NameOf;
            this.DateOfBirth = Date;
            this.Country = Country;
        }
    }
}
