using LibSoft.OdataAPI.Api.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibSoft.OdataAPI.Api.Controllers
{
    // Control OData'da Olduğu için Siliyoruz.
    /*
    [Route("api/[controller]")]
    [ApiController]
    */
    //[ODataRoutePrefix("Products")]
    // aksi halde her endpoint'in başına [ODataRoute("Products({key})")] eklemek gerekecek.
    [ODataRoutePrefix("Products")]
    // Kullanacağımız metotlar HTTP-GET Alıyorsa : Funtion Method
    // Kullanacağımız metotlar HTTP-POST Alıyorsa : Action Metot; olur.
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _appDbContext;
       
        public ProductsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [EnableQuery(PageSize = 2)]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Products.AsQueryable());
        }

        [HttpGet]
        [ODataRoute("({key})")]
        [EnableQuery]
        public IActionResult GetProduct([FromODataUri] int key)
        {
            return Ok(_appDbContext.Products.Where(x => x.Id == key));

            // Geriye IQuerable döner ve performans açısından çok uygundur. Performans açısından uygunluğunu test edebilmek için, MSSQL kullanılıyor ise SQL Profiler'dan görebilirsiniz. IEnumerable değeri liste şeklinde IQurable query şeklinde veri dönderdiği için performansı artırır.
        }

        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
            return Ok(product);
        }


        [HttpPut]
        public IActionResult PutProduct([FromODataUri] int key, [FromBody] Product product)
        {
            product.Id = key;
            _appDbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _appDbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct([FromODataUri] int key)
        {
            var product = _appDbContext.Products.Find(key);
            if(product == null)
            {
                return NotFound();
            }
            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChanges();
            return NoContent();
        }
    }
}
