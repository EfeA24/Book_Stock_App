using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contrats
{
    public interface IBookService
    {
        IEnumerable<Books> GetAllBooks(bool trackChanges);
        Books GetBookById(int id, bool trackChanges);
        Books CreateOneBooks(Books book);
        void UpdateOneBook(int id, Books book, bool trackChanges);
        void DeleteOneBook(int id, bool trackChanges);
    }
}
