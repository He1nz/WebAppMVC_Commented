using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBooks
{
    public class BookDTO
    //Data Transfer Objekt für Bücher
    {
        public string? Titel { get; set; }
        public string? Author { get; set; }
    }
}
