using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL
{
    public class ShowOrder
    {
        public IList<ShowOrderModel> GetShowingOrders()
        {
           DAL.Product pro = new DAL.Product();
            IList<ShowOrderModel> lst = pro.GetShowingOrders();
            return lst;
        }
    }
}
