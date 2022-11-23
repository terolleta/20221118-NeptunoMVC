using Microsoft.AspNetCore.Mvc;
using _20221118_NeptunoMVC.Models;

namespace _20221118_NeptunoMVC.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            CategoriesModel cm = new CategoriesModel();
            var lista = cm.GetCategories();

            ViewData["categories"] = lista;

            return View();
        }
    }
}
