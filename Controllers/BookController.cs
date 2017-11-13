using Microsoft.AspNetCore.Mvc;
using BookSack.Models;

namespace BookSack.Controllers
{

    /// <inheritdoc />
    /// <summary>
    /// API for Books
    /// </summary>
    [Route("api/[controller]")]
    public sealed class BookController : Controller
    {
        /// <summary>
        /// API method for inserting training book data.
        /// </summary>
        /// <param name="book">The Book passed to the API as a JSON object.</param>
        /// <returns>An ObjectResult with the newly created Book object's subgenre.</returns>
        [HttpPost]
        public ObjectResult Test([FromBody] Book book) => new ObjectResult(book.Subgenre);
    }
}