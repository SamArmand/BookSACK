using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Controllers
{

    /// <summary>
    /// The Controller for all views within the Home view.
    /// </summary>
    [Route("api/[controller]")]
    public class TrainingBookController : Controller
    {

                /// <summary>
        /// API method for inserting training book data.
        /// </summary>
        /// <param name="synopsis">The summary of the new TrainingBook to be created.</param>
        /// <param name="subcategory">The subcategory of the new TrainingBook to be created.</param>
        /// <returns>An ObjectResult with the newly created TrainingBook object.</returns>
        [HttpPost]
        public ObjectResult Insert([FromBody] TrainingBook trainingBook)
        {
            return new ObjectResult(StaticFunctions.CreateTrainingBook(trainingBook.Synopsis, trainingBook.Subcategory));
        }

        public ObjectResult Insert([FromBody] string synopsis) 
        {
            return new ObjectResult(new Book(){Synopsis = synopsis});
        }


    }

}