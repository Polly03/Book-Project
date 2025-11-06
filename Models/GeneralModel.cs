using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabase.Models
{
    public class GeneralModel
    {
        public string Name { get; set; }

        public GeneralModel(string name)
        {
            this.Name = name;
        }
    }
}
