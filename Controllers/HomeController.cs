using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;
using MysteriousDataProduct.Architecture;
using System;

namespace MysteriousDataProduct.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var book = new Book();
            book.Summary = "";
            return View("Index", book);
        }

        public IActionResult Trainer()
        {
            var trainingBook = new TrainingBook();
            trainingBook.Summary = "";
            return View("Trainer", trainingBook);
        }



        public IActionResult Create(string summary)
        {
            var book = new Book();
            book.Summary = summary;
            return View("Index", book);

        }

        public IActionResult Train(string summary, string subcategory)
        {
            
            var trainingBook = new TrainingBook();
            
            trainingBook.Subcategory = subcategory;

            if (summary == null)
            {
                trainingBook.Summary = "";
                return View("Trainer", trainingBook);    
            }

            trainingBook.Summary = summary;
            

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

            return View("Trainer", trainingBook);

        }

        public IActionResult About()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
