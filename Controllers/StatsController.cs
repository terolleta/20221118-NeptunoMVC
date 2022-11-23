using _20221118_NeptunoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _20221118_NeptunoMVC.Controllers
{
    public class StatsController : Controller
    {
        public IActionResult Top5()
        {
            StatModel sm = new StatModel();
            var top5 = sm.GetTop5();

            ViewData["Empleados"] = "[";
            ViewData["Amounts"] = "[";

            foreach (var item in top5)
            {
                ViewData["Empleados"] += "'" + item.EmployeeName + "', ";
                ViewData["Amounts"] += ((decimal)item.Amount).ToString("N2").Replace(".","").Replace(",", ".") + ", ";
            }
            ViewData["Empleados"] = ViewData["Empleados"].ToString().Remove(ViewData["Empleados"].ToString().Length - 2);
            ViewData["Amounts"] = ViewData["Amounts"].ToString().Remove(ViewData["Amounts"].ToString().Length - 2);
            ViewData["Empleados"] += "]";
            ViewData["Amounts"] += "]";

            //Preparacion de viewdata
            //['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange']  - empleados
            //[12, 19, 3, 5, 2, 3] - importes

            return View();
        }
    }
}
