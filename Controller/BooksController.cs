// Controllers/BooksController.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books = new List<Book>
        {
            new Book(1, "The Great Gatsby", "F. Scott Fitzgerald", 1925),
            new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960)
        };

        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            return Ok(_books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            book.Id = _books.Count + 1;
            _books.Add(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
    }
}
