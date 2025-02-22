﻿using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Services.Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
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
                var books = _manager.BookService.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = _manager
                .BookService
                .GetBookById(id, false);
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

            _manager.BookService.CreateOneBooks(bookEntity);

            return CreatedAtAction(nameof(GetOneBook), new { id = bookEntity.Id }, BookToDTO(bookEntity));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute] int id, [FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingBook = _manager.BookService.GetBookById(id, trackChanges: true);
            if (existingBook == null)
                return NotFound("Book not found");

            existingBook.Name = bookDto.Name;
            existingBook.Price = (decimal)bookDto.Price;

            _manager.BookService.UpdateOneBook(id, existingBook, trackChanges: true);

            return Ok(BookToDTO(existingBook));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            var book = _manager.BookService.GetBookById(id, trackChanges: false);
            if (book == null)
                return NotFound("Can't Find Book");

            _manager.BookService.DeleteOneBook(id, trackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<BookDTO> bookPatch)
        {
            if (id <= 0)
                return BadRequest("Invalid Book Id");

            var entity = _manager.BookService.GetBookById(id, trackChanges: true);
            if (entity == null)
                return NotFound("Can't Find Book");

            var bookDto = BookToDTO(entity);
            bookPatch.ApplyTo(bookDto);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            entity.Name = bookDto.Name;
            entity.Price = bookDto.Price.HasValue ? bookDto.Price.Value : entity.Price;

            _manager.BookService.UpdateOneBook(id, entity, trackChanges: true);

            return Ok(BookToDTO(entity));
        }
    }
}
