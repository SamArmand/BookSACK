using Microsoft.AspNetCore.Mvc;
using BookSack.Models;

namespace BookSack.Controllers
{

    /// <summary>
    /// The Controller for all views within the Home view.
    /// </summary>
    [Route("api/[controller]")]
    public class BookController : Controller
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