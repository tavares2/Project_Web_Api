using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_Code_Challenge.Contracts
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
