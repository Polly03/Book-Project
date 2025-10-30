using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabase.Models
{
    public class Authors
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string NameOf {  get; set; }
        public string Date { get; set; }
        public string Country { get; set; }

        public Authors (int Width, int Height, string NameOf, string Date, string Country)
        {
            this.Height = Height;
            this.Width = Width;
            this.NameOf = NameOf;
            this.Date = Date;
            this.Country = Country;
        }
    }
}
