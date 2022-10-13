using WebAppMVC.Utils;
using WebAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using DBBooks;
using System.Xml.Serialization;
using System;

namespace WebAppMVC.Controllers
{
    public class ControllerBooks : Controller
    {
        private readonly ConfigReader _configReader;

        public ControllerBooks(ConfigReader configReader)
        {
            this._configReader = configReader;
        }

        public string GetConnectionString()
        //ConnectionString wird ausgelesen
        {
            return _configReader.ReadConnectionString(); 
        }

        public IActionResult Index()
        //Daten aus Connection String werden an Model übergeben, View wird erstellt
        {
            ListModelBooks model = ReadInModel(GetConnectionString()); 
            return View(model); 
        }

        public ListModelBooks ReadInModel(string connectionString)
        //Zwei Listen werden erstellt und mit den Daten der Datenbanktabelle gefüllt
        {            
            List<BookDTO> currentBooks = new();
            List<BookDTO> archivedBooks = new();            
            var repository = new BooksRepo(connectionString);            
            Thread ReadCurrentBooks = new(() =>
            {
                currentBooks = repository.GetCurrentBooks();
            });
            Thread ReadArchivedBooks = new(() =>
            {
                archivedBooks = repository.GetArchivedBooks();
            });            
            ReadCurrentBooks.Start();
            ReadArchivedBooks.Start();
            ReadCurrentBooks.Join();
            ReadArchivedBooks.Join();
            return new ListModelBooks(currentBooks, archivedBooks);
        }

        public void Verschieben(BookDTO book, string source, string destination)
        //Buch wird verschoben -> aus der einen Tabelle gelöscht, in die andere Tabelle eingefügt
        {
            string connectionString = GetConnectionString();           
            var repository = new BooksRepo(connectionString);
            repository.MoveBook(book, source, destination); 
        }

        public IActionResult VerschiebeNachAktuell(BookDTO book)
        //Verschieben wird gestartet, dargestellte Tabellen werden aktualisiert
        {
            Verschieben(book, "archivierte_buecher", "aktuelle_buecher");
            ListModelBooks model = ReadInModel(GetConnectionString());
            return View("Views/Buecher/Index.cshtml", model); 
        }

        public IActionResult VerschiebeNachArchiviert(BookDTO book)
        //Verschieben wird gestartet, dargestellte Tabellen werden aktualisiert
        {
            Verschieben(book, "aktuelle_buecher", "archivierte_buecher"); 
            ListModelBooks model = ReadInModel(GetConnectionString()); 
            return View("Views/Buecher/Index.cshtml", model); 
        }
    }
}
