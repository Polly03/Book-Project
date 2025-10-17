using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace BookDatabase
{
    internal class Author
    {
        [Key]
        private int ID { get; set; }
        [MaxLength(64)]
        private string? Name { get; set; }
        private DateTime Date_of_birth {  get; set; }
        private int CountryID { get; set; }
        private string? AboutAuthor { get; set; }
    }
}
