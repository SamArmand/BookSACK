using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Controllers
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
        /// <param name="synopsis">The summary of the new TrainingBook to be created.</param>
        /// <param name="subcategory">The subcategory of the new TrainingBook to be created.</param>
        /// <returns>An ObjectResult with the newly created Book object.</returns>
        [HttpPost]
        public ObjectResult Test([FromBody] Book book) 
        {
            return new ObjectResult(book.Subcategory);
        }


    }

}