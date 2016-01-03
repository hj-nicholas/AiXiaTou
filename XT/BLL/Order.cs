using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL
{
   public class Order
    {
        DAL.Order order = new DAL.Order();

        public BaseResult AddOrderDesc(T_User_Order_Desc orderDTO)
       {
           BaseResult br= order.AddOrderDesc(orderDTO);
            return br;
       }

       public BaseResult updOrderSts(int orderId, int sts)
       {
           BaseResult br = order.UpdOrderSts(orderId, sts);
            return br;
       }

       public bool IsExistsLottery(string lotteryNo, int periodId)
       {
           bool flag = order.IsExistsLottery(lotteryNo, periodId);
           return flag;
       }
    }
}
