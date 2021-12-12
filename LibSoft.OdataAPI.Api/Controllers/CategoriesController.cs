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

    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _appDbContext;

        public CategoriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Categories);
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("Categories({id})/Products({item})")]
        public IActionResult GetProductById([FromODataUri] int id, [FromODataUri] int item)
        {
            return Ok(_appDbContext.Products.Where(x => x.CategoryId == id && x.Id == item));
        }

        [HttpPost] // Action Metotlar HttpPost ile çalışır
        public IActionResult TotalProductPrice([FromODataUri] int key)
        {
            var total = _appDbContext.Products.Where(x => x.CategoryId == key).Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost] // Action Metotlar HttpPost ile çalışır.
        public IActionResult TotalProductPriceCollect()
        {
            var total = _appDbContext.Products.Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost] // Action Metotlar HttpPost ile çalışır. Parametre alır. parametre adı startup'ta tanımlanan "categoryId" property'sidir.
        public IActionResult TotalProductPriceParam(ODataActionParameters parameters)
        {
            int categoryId = (int)parameters["categoryId"];
            var total = _appDbContext.Products.Where(x => x.CategoryId == categoryId).Sum(x => x.Price);
            return Ok(total);
        }


        [HttpPost] // Action Metotlar HttpPost ile çalışır. Parametre alır. Buradaki olay, startup tarafındaki belirtilen parametreyi yakalayıp toplatmak.
        public IActionResult Total(ODataActionParameters parameters)
        {
            int a = (int)parameters["a"];
            int b = (int)parameters["b"];
            int c = (int)parameters["c"];

            return Ok(a + b + c);
        }



    }
}
