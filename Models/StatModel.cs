using _20221118_NeptunoMVC.Dal;
using System.Collections.Generic;
using System.Linq;

namespace _20221118_NeptunoMVC.Models
{
    public class StatModel
    {
        public List<Top5Model> GetTop5()
        {
            var dbContext = new cifo_TSOContext();
            var lista = dbContext.OrderDetails
                .GroupBy(od=> new
                {
                    EmployeeName = od.Order.Employee.TitleOfCourtesy + " " + od.Order.Employee.LastName,
                }) 
                .Select( g => new Top5Model
                {
                    EmployeeName = g.Key.EmployeeName,
                    Amount = g.Sum
                    (
                        d=>d.Quantity * d.UnitPrice * (1-(decimal)d.Discount)
                    )
                })
                .OrderByDescending(g=>g.Amount)
                .Take(5)
                .ToList();                


            return lista;
        }
    }

    public class Top5Model
    {
        public string EmployeeName { get; set; }
        public decimal? Amount { get; set; }
    }
}
