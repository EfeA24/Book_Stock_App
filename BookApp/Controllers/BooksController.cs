using BookApp.DTO;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Threading.Tasks;

namespace BooksDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        private static BookDTO BookToDTO(Books book)
        {
            return new BookDTO
            {
                Name = book.Name,
                Price = book.Price,
            };
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);
                return Ok(books);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = _manager
                .Book
                .GetOneBookById(id, false);
            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookEntity = new Books
            {
                Name = bookDto.Name,
                Price = (decimal)bookDto.Price
            };

            _manager.Book.CreateOneBook(bookEntity);
            _manager.Save();

            return CreatedAtAction(nameof(GetOneBook), new { id = bookEntity.Id }, BookToDTO(bookEntity));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute] int id, [FromBody] BookDTO bookDto)
        {
            var entity = _manager.Book.GetOneBookById(id, trackChanges: true);
            if (entity == null)
                return NotFound();

            entity.Name = bookDto.Name;
            entity.Price = (decimal)bookDto.Price;

            _manager.Book.UpdateOneBook(entity);
             _manager.Save();

            return Ok(BookToDTO(entity));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            var book =_manager.Book.GetOneBookById(id, trackChanges: true);
            if (book == null)
                return NotFound("Can't Find Book");

            _manager.Book.DeleteOneBook(book);
            _manager.Save();

            return Ok("Book Deleted Successfully");
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<BookDTO> bookPatch)
        {
            if (id <= 0)
                return BadRequest("Invalid Book Id");

            var entity =_manager.Book.GetOneBookById(id, trackChanges: true);
            if (entity == null)
                return NotFound("Can't Find Book");

            var bookDto = BookToDTO(entity);
            bookPatch.ApplyTo(bookDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            entity.Name = bookDto.Name;
            entity.Price = (decimal)bookDto.Price;

            _manager.Book.UpdateOneBook(entity);
            _manager.Save();

            return Ok(BookToDTO(entity));
        }
    }
}