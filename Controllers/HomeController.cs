using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;
using MysteriousDataProduct.Architecture;
using System;

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
        /// <param name="subcategory">The subcategory of the new TrainingBook to be created. Defaults to an empty string.</param>
        /// <returns>Returns a ViewResult with the Trainer view and TrainingBook model to be loaded.</returns>
        public ViewResult Trainer(string synopsis = "", string subcategory = "")
        {
            return View("Trainer", CreateTrainingBook(synopsis, subcategory));
        }

        /// <summary>
        /// API method for inserting training book data.
        /// </summary>
        /// <param name="synopsis">The summary of the new TrainingBook to be created.</param>
        /// <param name="subcategory">The subcategory of the new TrainingBook to be created.</param>
        /// <returns>An ObjectResult with the newly created TrainingBook object.</returns>
        [Route("api/{synopsis}/{subcategory}")]
        public ObjectResult Insert(string synopsis, string subcategory)
        {
            return new ObjectResult(CreateTrainingBook(synopsis, subcategory));
        }

        [Route("api/{synopsis}")]
        public ObjectResult Insert(string synopsis) 
        {
            return new ObjectResult(new Book(){Synopsis = synopsis});
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

        /// <summary>
        /// Private method that handles the logic of creating a TrainingBook object and inserting it into the database.
        /// </summary>
        /// <param name="synopsis">The summary of the new TrainingBook to be created.</param>
        /// <param name="subcategory">The subcategory of the new TrainingBook to be created.</param>
        /// <returns>Returns a new TrainingBook.</returns>
        private TrainingBook CreateTrainingBook(string synopsis, string subcategory) {

            var trainingBook = new TrainingBook() {
                Synopsis = synopsis,
                Subcategory = subcategory
            };

            if (synopsis == null || synopsis == "")          
                RedirectToAction("Trainer", trainingBook);

            var dataAccess = new DataAccess();

            var dictionary = dataAccess.GetDictionaries()[subcategory];

            foreach (var kvp in trainingBook.SortedWordFrequency) {

                if (!dictionary.ContainsKey(kvp.Key)) {

                    var Word = new Word() {

                        WordString = kvp.Key,
                        Subcategory = subcategory,
                        FrequencyPlus1 = 1

                    };

                    dictionary.Add(Word.WordString, Word);

                }

                dictionary[kvp.Key].FrequencyPlus1 += kvp.Value;             

            }

            float sum = 0f;

            foreach (var kvp in dictionary)
                sum += kvp.Value.FrequencyPlus1;

            foreach (var kvp in dictionary) {

                kvp.Value.Probability = (float)kvp.Value.FrequencyPlus1 / sum;
                kvp.Value.Ln = Math.Log(kvp.Value.Probability);

            }

            dataAccess.Update(subcategory, dictionary);

            return trainingBook;

        }


    }
}
