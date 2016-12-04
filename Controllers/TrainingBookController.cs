using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Architecture;
using MysteriousDataProduct.Models;

namespace MysteriousDataProduct.Controllers
{
    /// <summary>
    /// The Controller for all views within the Home view
    /// </summary>
    [Route("api/[controller]")]
    public class TrainingBookController : Controller
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