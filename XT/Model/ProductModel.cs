using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public int ProductType { get; set; }
        public int PeriodId { get; set; }
        public string PeriodNum { get; set; }
        public decimal ProductPrice { get; set; }
        public int JoinedNum { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime ProductExpires { get; set; }
    }
}
