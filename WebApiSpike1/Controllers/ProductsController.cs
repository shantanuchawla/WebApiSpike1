using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSpike1.Models;

namespace WebApiSpike1.Controllers
{
    public class ProductsController : ApiController
    {
        static List<Product> products = new List<Product> 
        { 
            new Product { Id = 1, Category = "Groceries", Price = 1 }, 
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys" }, 
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult Post(Product p)
        {
            products.Add(p);
            Console.WriteLine(ControllerContext.Request.ToString());
            Console.WriteLine("{0}\t{1};\t{2}\t{3}", p.Name, p.Id, p.Price, p.Category);
            return Ok();
        }
    }
}
