using Microsoft.AspNetCore.Mvc;
using _20221118_NeptunoMVC.Models;
using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace _20221118_NeptunoMVC.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index(int CategoryId = 0, string search ="")
        {
            //TO_DO
            //recoger paràmetros del listado (catid o texto)
            //de momento llevamos el paràmetro cateogria al model...

            if (search == null) search = "";
            
            ProductsModel pm = new ProductsModel();

            //Preparar objetos filtro (desplegable cat y caja de texto)
            var categories = pm.GetCategories();
            ViewData["cmbCategorias"] = "<select id='cmbCategorias'>";
            ViewData["cmbCategorias"] += "<option value='0'>All</option>";
            foreach (var category in categories) 
            {
                ViewData["cmbCategorias"] += "<option value='"+category.CategoryId+"'" + 
                    ((category.CategoryId==CategoryId)? " selected ": "")+">" + category.CategoryName+"</option>";
            }

            ViewData["txtTexto"] = "<input type='text' id='txtSearch' value='"+search+"'/>";
            
            
            var lista = pm.GetProducts(CategoryId, search);

            ViewData["Products"] = lista;

            return View();
        }

        public IActionResult Detail(int ProductId =0) 
        {
            ProductsModel pm = new ProductsModel();
            var product = pm.GetProduct(ProductId);

            //Preparar imagen
            string imagen = "";
            string nombreImagen = product.ProductId + "lr.jpg";
            string nombreImagenHR = product.ProductId + "hr.jpg";
            string path = (string)AppDomain.CurrentDomain.GetData("WebRootPath") + "wwwroot\\Images\\Products\\" + nombreImagen;
            if (System.IO.File.Exists(path))
            {
                imagen = "<a href='/Images/Products/" + nombreImagenHR + "' " +
                            "data-lightbox='image-" + product.ProductId + "' " +
                            "data-title='" + product.ProductName + "'>" +
                            "<img src='/Images/Products/" + nombreImagen + "' width = '100%'/>" +
                        "</a>";
            }
            else
            {
                imagen = "<img src='/Images/sinimagen.png' width = '100%'/>";
            }

            //Preprar viewdata
            ViewData["imagen"] = imagen;
            ViewData["categoria"] = product.CategoryName;
            ViewData["nombreProducto"] = product.ProductName;
            ViewData["precio"] = ((decimal)product.Price).ToString("N2") + " €";
            ViewData["descripcion"] = "";
            ViewData["ProductId"] = ProductId;


            if (product.Stock == 0)
            {
                ViewData["stock"] ="<i class=\"fa fa-thermometer-empty\" aria-hidden=\"true\" style='color:red;font-size:1.5em;' title='no stock " + product.Stock + "'></i>";
            }
            else if (product.Stock < 10)
            {
                ViewData["stock"] = "<i class=\"fa fa-thermometer-quarter\" aria-hidden=\"true\" style='color:orange;font-size:1.5em;' title='low stock'></i>";
            }
            else if (product.Stock < 20)
            {
                ViewData["stock"] = "<i class=\"fa fa-thermometer-half\" aria-hidden=\"true\" style='color:cyan;font-size:1.5em;' title='medium stock'></i>";
            }
            else if (product.Stock > 20)
            {
                ViewData["stock"] = "<i class=\"fa fa-thermometer-full\" aria-hidden=\"true\" style='color:green;font-size:1.5em;' title='high stock'></i>";
            }

            ViewData["ventas"] = ((decimal)product.Sales).ToString("N2") + " €";

            return View();
        }

        //Ajax
        public string ChangeState(int productId)
        {
            ProductsModel pm = new ProductsModel();
            var resp = pm.ChangeState(productId);

            return resp;
        }

        public string GetHistorico(int productId)
        {
            string result = "";

            ProductsModel pm = new ProductsModel();
            var historico = pm.GetHistorico(productId);

            //Cabezera de la tabla
            result = @"<div class = 'table-responsive'>
                    <table class='table table-bordered table-striped'>
                        <tr>
                            <th> OrderId </th >
                            <th> Fecha </th>
                            <th> Customer </th>
                            <th> Employee </th>
                            <th> Quantity </th>
                            <th> Precio </th>
                            <th> Descuento </th>
                            <th> Precio[€] </th>
                        </ tr >";

            //Contenido de la tabla
            decimal? total = 0;
            foreach ( var item in historico)
            {
                result += "<tr>";

                result += "<td>" + item.OrderId;
                result += "</td>";

                result += "<td>" + ((DateTime)item.OrderDate).ToShortDateString();
                result += "</td>";

                result += "<td>" + item.CustomerName;
                result += "</td>";

                result += "<td>" + item.Employee;
                result += "</td>";

                result += "<td>" + item.Quantity;
                result += "</td>";

                result += "<td>" + ((decimal)item.Price).ToString("N2");
                result += "</td>";

                result += "<td>" + item.Discount;
                result += "</td>";

                result += "<td>" + ((decimal)item.Amount).ToString("N2") ;
                result += "</td>";

                result += "</tr>";

                total += item.Amount;
            }

            //Fila de totales
            result += "<tr><td colspan='7' style='text-align:right'>TOTAL:</td>";
            result += "<td style='font-weight:bold;'>" + ((decimal)total).ToString("N2") + "</td></tr>";

            result += "</table></div>";
            return result;
        }

        public string GetSupplier(int productId)
        {
            ProductsModel pm = new ProductsModel();
            var supplier = pm.GetSupplier(productId);

            string resp = supplier.ContactName + " " + supplier.ContactName;

            return resp;
        }
    }
}
