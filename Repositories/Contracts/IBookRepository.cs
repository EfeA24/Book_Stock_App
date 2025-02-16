using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Books>
    {
        IQueryable<Books> GetAllBooks(bool trackChanges);
        Books GetOneBookById(int id, bool trackChanges);
        void CreateOneBook(Books book);
        void UpdateOneBook(Books book);
        void DeleteOneBook(Books book);
    }
}
