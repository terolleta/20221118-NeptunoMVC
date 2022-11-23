using _20221118_NeptunoMVC.Dal;
using System.Collections.Generic;
using System.Linq;

namespace _20221118_NeptunoMVC.Models
{
    public class CategoriesModel
    {
        public List<Category> GetCategories()
        {
            var dbContext = new cifo_TSOContext();

            var lista = dbContext.Categories
                .OrderBy(c=> c.CategoryName)
                .ToList();

            return lista;
        }
    }

   
}
