using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibSoft.OdataAPI.Api.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        // [ForeignKey("Category")] // Entity yapılacak ekstradan bir yapı olacaksa.
        public int CategoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual Category Category { get; set; }
    }
}
