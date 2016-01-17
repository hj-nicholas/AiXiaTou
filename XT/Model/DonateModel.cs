using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DonateModel
    {
        public string City  {get;set;}

        public string PhotoPath { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public int PeriodID { get; set; }
        public int ShareNum { get; set; }
    }
}
