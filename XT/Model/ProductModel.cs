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

        public string ProLotteryNum { get; set; }

        public string UserName { get; set; }
        //用户虾仔码
        public List<string> UserLots { get; set; }
        //开奖时间
        public DateTime OpenTime { get; set; }

        public int UserId { get; set; }
        //是否晒单
        public int IsShowOrder { get; set; }

        public decimal ActualPrice { get; set; }

        public string ProductUrl { get; set; }

    }
}
