using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;
using MysteriousDataProduct.Architecture;

namespace MysteriousDataProduct.Controllers
{

    /// <summary>
    /// The Controller for all views within the Home view.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Creates a Book model and returns the Index view with that model.
        /// </summary>
        /// <param name="synopsis">The synopsis of the new Book to be created. Defaults to an empty string resulting in an empty Book.</param>
        /// <returns>Returns a ViewResult with the Index view and Book model to be loaded.</returns>
        public ViewResult Index(string synopsis = "")
        {
            return View("Index", new Book() {Synopsis = synopsis});
        }

        /// <summary>
        /// Creates a TrainingBook model and returns the Trainer view with that model.
        /// </summary>
        /// <param name="synopsis">The synopsis of the new TrainingBook to be created. Defaults to an empty string resulting in an empty TrainingBook.</param>
        /// <param name="subgenre">The subcategory of the new TrainingBook to be created. Defaults to an empty string.</param>
        /// <returns>Returns a ViewResult with the Trainer view and TrainingBook model to be loaded.</returns>
        public ViewResult Trainer(string synopsis = "", string subgenre = "")
        {
            return View("Trainer", new TrainingBook{Synopsis = synopsis, Subgenre = subgenre});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns a ViewResult with the About view.</returns>
        public ViewResult About()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns a ViewResult with the About view.</returns>
        public ViewResult Error()
        {
            return View();
        }



    }
}
