using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
    public class BookRepository : RepositoryBase<Books>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public void CreateOneBook(Books book)
        {
            Create(book);
        }

        public void DeleteOneBook(Books book)
        {
            Delete(book);
        }

        public IQueryable<Books> GetAllBooks(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderBy(x => x.Id);

        }

        public IQueryable<Books> GetAllBooksById(int id, bool trackChanges)
        {
            return FindByCondition(b => b.Id == id, trackChanges)
                .OrderBy(x => x.Id);
        }

        public void UpdateOneBook(Books book)
        {
            Update(book);
        }
    }
}
