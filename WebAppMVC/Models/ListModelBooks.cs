using DBBooks;

namespace WebAppMVC.Models
{
    public class ListModelBooks
    //Listen werden erstellt und befüllt
    {        
        public List<BookDTO> CurrentBooksList { get; set; } = new();
        public List<BookDTO> ArchivedBooksList { get; set; } = new();
        public ListModelBooks(IEnumerable<BookDTO> currentBooks, IEnumerable<BookDTO> archivedBooks)
        {            
            foreach(BookDTO bookDTO in currentBooks)
            {
                CurrentBooksList.Add(bookDTO);
            }
            foreach (BookDTO bookDTO in archivedBooks)
            {
                ArchivedBooksList.Add(bookDTO);
            }
        }
    }
}
