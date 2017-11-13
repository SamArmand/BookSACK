using Microsoft.AspNetCore.Mvc;
using BookSack.Architecture;
using BookSack.Models;

namespace BookSack.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// API for Training Books
    /// </summary>
    [Route("api/[controller]")]
    public sealed class TrainingBookController : Controller
    {
        /// <summary>
        /// API method for inserting training book data
        /// </summary>
        /// <param name="trainingBook">The TrainingBook passed to the API to be created.</param>
        /// <returns>An ObjectResult with the value true.</returns>
        [HttpPost]
        public ObjectResult Train([FromBody] TrainingBook trainingBook) => (new ObjectResult(trainingBook));

        /// <summary>
        /// API method for resetting training book data
        /// </summary>
        /// <returns>An ObjectResult with the value true.</returns>
        [HttpDelete]
        public ObjectResult Reset()
        {
            DataAccess.Reset();
            return new ObjectResult(true);
        }
    }
}