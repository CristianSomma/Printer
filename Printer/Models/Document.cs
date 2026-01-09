using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Models
{
    public class Document
    {
        private string _name;
        private int _pages;

        public Document(string name, int pages)
        {
            Name = name;
            Pages = pages;
        }

        public string Name
        {
            get => _name;
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("The file name cannot be null or just whitespaces.");

                _name = value.ToUpper();
            }
        }

        public int Pages
        {
            get => _pages;
            private set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("The document has to be at least one page long.");

                _pages = value;
            }
        }
    }
}
