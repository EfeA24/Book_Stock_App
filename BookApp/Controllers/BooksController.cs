using BookApp.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.EfCore;

namespace BooksDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        private static BookDTO BookToDTO(Books books)
        {
            return new BookDTO
            {
                Name = books.Name,
                Price = books.Price,
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBook([FromRoute(Name = "id")] int id)
        {

            var book = await _context.Books
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetBookByName([FromRoute(Name = "name")] string name)
        {

            var book = await _context.Books
                .Where(i => i.Name == name)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO booksave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookEntity = new Books
            {
                Name = booksave.Name,
                Price = (decimal)booksave.Price
            };

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = bookEntity.Id }, BookToDTO(bookEntity));
        }




        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookDTO bookDTO)
        {
            var entity = await _context.Books.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = bookDTO.Name;
            entity.Price = (decimal)bookDTO.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(BookToDTO(entity));
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            if(id == null)
            {
                return NotFound("Can't Find Id");
            }

            var book = await _context.Books.FirstOrDefaultAsync(i => i.Id == id);

            if(book == null)
            {
                return NotFound("Can't Find Book");
            }
            _context.Books.Remove(book);

            try
            {
                await _context.SaveChangesAsync();
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return StatusCode(200, "Book Deleted Successfully");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<Books> bookPatch)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Book Id");
            }

            var entity = await _context.Books.FirstOrDefaultAsync(i => i.Id == id);

            if (entity == null)
            {
                return NotFound("Can't Find Book");
            }

            bookPatch.ApplyTo(entity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return Ok(entity);
        }
    }
}
