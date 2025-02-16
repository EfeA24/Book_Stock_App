using Entities.Models;
using Repositories.Contracts;
using Services.Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Books CreateOneBooks(Books book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();

            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity = _manager.Book.GetOneBookById(id, trackChanges);
            if (entity == null)
            {
                throw new Exception($"Book with id:{id} could not found");
            }
            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Books> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges).ToList();
        }

        public Books GetBookById(int id, bool trackChanges)
        {
            return _manager.Book.GetOneBookById(id, trackChanges);
        }

        public void UpdateOneBook(int id, Books book, bool trackChanges)
        {
            var entity = _manager.Book.GetOneBookById(id, trackChanges);
            if (entity == null)
            {
                throw new Exception($"Book with id:{id} could not found");
            }
            if(book == null)
            {
                throw new Exception("Book is null");
            }
            
            entity.Name = book.Name;
            entity.Price = book.Price;

            _manager.Book.Update(entity);
            _manager.Save();
        }
    }
}
