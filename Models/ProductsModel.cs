using _20221118_NeptunoMVC.Dal;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _20221118_NeptunoMVC.Models
{
    public class ProductsModel
    {
        public List<ProductSlim> GetProducts(int categoryId, string search)
        {
            var dbContext = new cifo_TSOContext();

            var lista = dbContext.Products
                .Where(p => (p.CategoryId == categoryId || categoryId == 0) && p.ProductName.Contains(search))
                .OrderBy(p => p.ProductName)
                .Select(p => new ProductSlim
                {
                    ProductName = p.ProductName,
                    ProductId = p.ProductId,
                    CategoryName = p.Category.CategoryName,
                    Price = p.UnitPrice,
                    Stock = p.UnitsInStock,
                    State = (p.Discontinued == 0) ? true : false
                })
                .ToList();

            return lista;
        }

        public string ChangeState(int productId)
        {
            var dbContext = new cifo_TSOContext();
            var product = dbContext.Products.Find(productId);

            product.Discontinued = (product.Discontinued == 0) ? (ulong)1 : (ulong)0;

            dbContext.SaveChanges();

            return (product.Discontinued == 0) ? "Active" : "Discontinued";

        }

        public List<CategorySlim> GetCategories()
        {
            var dbContext = new cifo_TSOContext();
            var categories = dbContext.Categories
                .OrderBy(c => c.CategoryName)
                .Select(c => new CategorySlim
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                }).ToList();

            return categories;
        }

        public ProductDetail GetProduct(int productId)
        {
            var dbContext = new cifo_TSOContext();
            ProductDetail p = dbContext.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductDetail
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Price = p.UnitPrice,
                    Stock = p.UnitsInStock,
                    Descripcion = p.QuantityPerUnit,
                    Sales =
                    (
                        dbContext.OrderDetails
                        .Where(od => od.ProductId == productId)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))

                    )
                }).First();



            return p;
        }

        public List<OrderDetailExtended> GetHistorico(int productId) 
        {
            var dbConext = new cifo_TSOContext();
            var lista = dbConext.OrderDetails
                .Where(od => od.ProductId == productId)
                .Select(od => new OrderDetailExtended
                {
                    OrderId = od.OrderId,
                    OrderDate = od.Order.OrderDate,
                    CustomerName = od.Order.Customer.CompanyName,
                    Employee = od.Order.Employee.TitleOfCourtesy + " " + od.Order.Employee.LastName,
                    Quantity = od.Quantity,
                    Price = od.UnitPrice,
                    Discount = od.Discount,
                    Amount = od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)
                })
                .OrderByDescending(od => od.OrderDate)
                .ToList();

            return lista;
        }

        public Supplier GetSupplier(int productId)
        {
            var dbContext = new cifo_TSOContext();
            var supplier = dbContext.Products
                .Where(p => p.ProductId == productId)
                .Select(s => new Supplier
                {
                    SupplierId = (int)s.SupplierId,
                    CompanyName = s.Supplier.CompanyName,
                    ContactName = s.Supplier.ContactName,
                    ContactTitle = s.Supplier.ContactTitle,
                    Address = s.Supplier.Address,
                    City = s.Supplier.City,
                    Region = s.Supplier.Region,
                    PostalCode = s.Supplier.PostalCode,
                    Country = s.Supplier.Country,
                    Phone = s.Supplier.Phone,
                    Fax = s.Supplier.Fax,
                    HomePage = s.Supplier.HomePage,
                }).First();

            return supplier;                

        }
    }

    public class ProductSlim
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal? Price { get; set; }
        public short? Stock { get; set; }
        public bool State { get; set; }
    }

    public class CategorySlim
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductDetail
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal? Price { get; set; }
        public short? Stock { get; set; }
        public string Descripcion { get; set; }
        public decimal? Sales { get; set; }

    }

    public class OrderDetailExtended
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string Employee { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public double Discount { get; set; }
        public decimal? Amount { get; set; }

    }
}
