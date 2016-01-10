using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class Product
    {
        DAL.Product pro = new DAL.Product();
        public IList<ProductModel> GetProducts(int proType,int userId)
        {
            IList<ProductModel> lst = pro.GetProducts(proType, userId);
            return lst;
        }

        public ProductModel GetProductByPeriodId(int periodId)
        {
            var product = pro.GetProductByPeriodId(periodId);
            return product;

        }

        public IList<T_ProductPeriods> GetPeriods(int productId)
        {
            var periods = pro.GetPeriods(productId);
            return periods;
        }

        public BaseResult GetRelPeriodInfo(int periodId, out int suppNum, out int commNum)
        {
            var result = pro.GetRelPeriodInfo(periodId, out suppNum, out commNum);
            return result;
        }

        public IList<UserOrderDTO> GetOrderList(int periodId)
        {
            var lst = pro.GetOrderList(periodId);
            return lst;
        }

        public IList<DonateModel> GetDonaterList(int periodId)
        {
            var lst = pro.GetDonaterList(periodId);
            return lst;
        }

        public IList<ProductModel> GetEnshrineProducts(int  userId)
        {
            IList<ProductModel> lst = pro.GetEnshrineProducts( userId);
            return lst;
        }

        public UserOrderDTO ConfirmLottery(string lotteryTicket, int periodId)
        {
            var userOrder = pro.ConfirmLottery(lotteryTicket,periodId);
            return userOrder;
        }

        public BaseResult AddGift(T_User_Share shareDto)
        {
            var result =  pro.AddGift(shareDto);
            return result;
        }

        public BaseResult UpdGiftSts(int shareId, int sts)
        {
            var result = pro.UpdGiftSts(shareId,sts);
            return result;
        }

        public T_User_Share GetShareById(int shareId)
        {
            var shareDto = pro.GetShareById(shareId);
            return shareDto;
        }

        public bool isRevGift(int shareId, int userId)
        {
            var flag = pro.isRevGift(shareId, userId);
            return flag;
        }

        public BaseResult UpdRevGiftInfo(int shareId, int userId, int RevNum, int periodId, string lotteryNO)
        {
            var result = pro.UpdRevGiftInfo(shareId, userId, RevNum,periodId,lotteryNO);
            return result;
        }
    }
}
