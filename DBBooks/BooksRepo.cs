using MySqlConnector;

namespace DBBooks
{
    public class BooksRepo
    {
        public string connectionString;

        public BooksRepo(string connectionString)
        {
            this.connectionString = connectionString; 
        }

        public List<BookDTO> GetCurrentBooks()
        //Aktuelle Bücher werden in Liste eingelesen
        {
            return GetBooksFromTable("aktuelle_Buecher"); 
        }

        public List<BookDTO> GetArchivedBooks()
        //Archivierte Bücher werden in Liste eingelesen
        {
            return GetBooksFromTable("archivierte_Buecher"); 
        }

        public List<BookDTO> GetBooksFromTable(string tablename)
        //Die Bücher aus der im Übergabeparameter spezifizierten Tabelle werden von der Datenbank abgerufen
        {
            List<BookDTO> Books = new();            
            using var dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            string SQLBooks = "SELECT titel, autor FROM " + tablename;
            using var commando = new MySqlCommand(SQLBooks, dbConnection);
            using var reader = commando.ExecuteReader();
            while (reader.Read())
            {
                BookDTO book = new()
                {
                    Author = (string?)reader["autor"],
                    Titel = (string?)reader["titel"]
                };
                Books.Add(book);
            }
            dbConnection.Close();
            return Books;
        }        

        public void DeleteBook(BookDTO book, string source)
        //Funktion zum Löschen eines Buches
        {
            using var dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            string SQLDeleteBook = "DELETE FROM " + source + " WHERE autor = @author AND titel = @titel";
            using var command = new MySqlCommand(SQLDeleteBook, dbConnection);
            command.Parameters.AddWithValue("author", book.Author);
            command.Parameters.AddWithValue("titel", book.Titel);            
            command.ExecuteNonQuery();
            dbConnection.Close();
        }

        public void InsertBook(BookDTO book, string destination)
        //Funktion zum Einfügen eines Buches
        {
            using var dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            string SQLInsertBook = "INSERT INTO " + destination + " (autor, titel) VALUES ('" + book.Author + "', '" + book.Titel + "')";
            using var command = new MySqlCommand(SQLInsertBook, dbConnection);
            command.Parameters.AddWithValue("autor", book.Author);
            command.Parameters.AddWithValue("titel", book.Titel);       
            command.ExecuteNonQuery();
            dbConnection.Close();
        }

        public void MoveBook(BookDTO book, string source, string destination)
        //Das Buch wird verschoben -> aus der einen Tabelle gelöscht, in die andere Tabelle eingefügt
        {
            DeleteBook(book, source);
            InsertBook(book, destination);
        }
    }
}