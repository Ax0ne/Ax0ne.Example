using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Example.Domain;
using Example.Domain.Models;
using Example.Domain.DTO;

namespace Example.Web.Controllers.WebApi
{
    public class ProductsController : ApiController
    {
        private readonly ExampleContext _db = new ExampleContext();

        // Project products to product DTOs. 
        // 把products投影到product DTOs
        private IQueryable<ProductDTO> MapProducts()
        {
            return from p in _db.Products
                   select new ProductDTO() { Id = p.Id, Name = p.Name, Price = p.Price };
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return MapProducts().AsEnumerable();
        }

        public ProductDTO GetProduct(int id)
        {
            var product = (from p in MapProducts()
                           where p.Id == 1
                           select p).FirstOrDefault();
            if (product == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return product;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region Comment Old
        /*
        static readonly IProductRepository repository = new ProductRepository();

        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }
        public HttpResponseMessage PostProduct1(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        /// <summary>
        /// 方法名以Put开头,就匹配Put请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        public HttpResponseMessage DeleteProduct(int id)
        {
            repository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        } 
        */
        #endregion

    }
}
