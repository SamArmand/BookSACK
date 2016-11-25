using Microsoft.AspNetCore.Mvc;
using MysteriousDataProduct.Models;

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

        public IActionResult Create(string summary)
        {
            var book = new Book();
            book.Summary = summary;
            return View("Index", book);

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
