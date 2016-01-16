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
        public IList<ShowOrderModel> GetShowingOrders(int productId,int userId,int ViewUserId)
        {
           DAL.Product pro = new DAL.Product();
            IList<ShowOrderModel> lst = pro.GetShowingOrders(productId, userId, ViewUserId);
            return lst;
        }

        public BaseResult AddShow(string content, int periodId, int userId)
        {
            DAL.UserInfo user = new DAL.UserInfo();
            BaseResult result= user.AddShow(content, periodId, userId);
            return result;
        }
    }
}
