using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common;
using Model;

namespace DAL
{
   public class Product
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        
        //晒单数据
        public IList<ShowOrderModel> GetShowingOrders(int productId,int userId,int ViewUserId)
       {
            try
            {
                IList<ShowOrderModel> lstShowOrder = new List<ShowOrderModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetShowOrder", strConn);
                cmd.Parameters["@p_productId"].Value = productId;
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_ViewUser"].Value = ViewUserId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        ShowOrderModel showOrder = new ShowOrderModel();
                        showOrder.CommentContent = Convert.ToString(rdr["CommentContent"]);
                        showOrder.CommentDate = Convert.ToDateTime(rdr["CommentDate"]);
                        showOrder.CommentID = Convert.ToInt32(rdr["CommentID"]);
                        showOrder.CommentNum = Convert.ToInt32(rdr["CommentNum"]);
                        showOrder.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        showOrder.ProductName = Convert.ToString(rdr["ProductName"]);
                        showOrder.SupportNum = Convert.ToInt32(rdr["SupportNum"]);
                        showOrder.UserName = Convert.ToString(rdr["UserName"]);
                        showOrder.PeriodID = Convert.ToInt32(rdr["PeriodId"]);
                        showOrder.UserImage = Convert.ToString(rdr["PhotoPath"]);
                        showOrder.IsSupp = Convert.ToInt32(rdr["IsSupp"]);
                        showOrder.City = Convert.ToString(rdr["City"]);
                        //照片信息
                        showOrder.Photos = GetImageByType(1, showOrder.PeriodID).ToList();

                        lstShowOrder.Add(showOrder);
                    }
                }

                return lstShowOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //产品列表
        public IList<ProductModel> GetProducts(int proType,int userId=0)
        {
            try
            {
                IList<ProductModel> lstProd = new List<ProductModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProducts", strConn);
                cmd.Parameters["@p_ProType"].Value = proType;
                cmd.Parameters["@p_UserId"].Value = userId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        ProductModel prod = new ProductModel();
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.UserName = Convert.ToString(rdr["UserName"]);
                        prod.ProductUrl = Convert.ToString(rdr["ProductUrl"]);
                        prod.ProductPhoto = Convert.ToString(rdr["PhotoPath"]);
                        if (!DBNull.Value.Equals(rdr["ActualPrice"]))
                            prod.ActualPrice = Convert.ToInt32(rdr["ActualPrice"]);
                        if (!DBNull.Value.Equals(rdr["IsShowOrder"]))
                            prod.IsShowOrder = Convert.ToInt32(rdr["IsShowOrder"]);
                        if (!DBNull.Value.Equals(rdr["UserID"]))
                            prod.UserId = Convert.ToInt32(rdr["UserID"]);
                        if (!DBNull.Value.Equals(rdr["OpenTime"]))
                            prod.OpenTime = Convert.ToDateTime(rdr["OpenTime"]);
                        if (userId!=0)
                            prod.UserLots = GetLotterysByUid(prod.PeriodId, userId).ToList();
                        lstProd.Add(prod);
                    }
                }

                return lstProd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //根据订单查询产品信息
        public ProductModel GetProdByOrder(int orderId)
        {
            ProductModel prod = new ProductModel();

            try
            {
               
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProdByOrder", strConn);
                cmd.Parameters["@p_orderId"].Value = orderId;
                
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.UserName = Convert.ToString(rdr["UserName"]);
                        if (!DBNull.Value.Equals(rdr["IsShowOrder"]))
                            prod.IsShowOrder = Convert.ToInt32(rdr["IsShowOrder"]);
                        if (!DBNull.Value.Equals(rdr["UserID"]))
                            prod.UserId = Convert.ToInt32(rdr["UserID"]);
                        if (!DBNull.Value.Equals(rdr["OpenTime"]))
                            prod.OpenTime = Convert.ToDateTime(rdr["OpenTime"]);
                    }
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prod;
        }
        //根据订单查询产品信息
        public ProductModel GetProdByShare(int shareId)
        {
            ProductModel prod = new ProductModel();

            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProdByShare", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.UserName = Convert.ToString(rdr["UserName"]);
                        if (!DBNull.Value.Equals(rdr["IsShowOrder"]))
                            prod.IsShowOrder = Convert.ToInt32(rdr["IsShowOrder"]);
                        if (!DBNull.Value.Equals(rdr["UserID"]))
                            prod.UserId = Convert.ToInt32(rdr["UserID"]);
                        if (!DBNull.Value.Equals(rdr["OpenTime"]))
                            prod.OpenTime = Convert.ToDateTime(rdr["OpenTime"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prod;
        }

        //获取用户该期活动的虾仔码
        public IList<string> GetLotterysByUid(int periodId, int userId)
        {
            try
            {
                IList<string> lstLot = new List<string>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetLotterysByUid", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_UserId"].Value = userId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                       string lot = Convert.ToString(rdr["LotteryNum"]);
                        //prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        //prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        //prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        //prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        //prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        //prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        //prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        //prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        //prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        //prod.UserName = Convert.ToString(rdr["UserName"]);
                        lstLot.Add(lot);
                    }
                }

                return lstLot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //查询某个产品的历史期数
        public IList<ProductModel> GetProductsByProId(int productId)
        {
            try
            {
                IList<ProductModel> lstProd = new List<ProductModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProductsByProId", strConn);
                cmd.Parameters["@p_productId"].Value = productId;
                
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        ProductModel prod = new ProductModel();
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.UserName = Convert.ToString(rdr["UserName"]);
                        lstProd.Add(prod);
                    }
                }

                return lstProd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductModel GetProductByPeriodId(int periodId)
        {
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProductByPeriodId", strConn);
                cmd.Parameters["@p_PeriodId"].Value = periodId;
                ProductModel prod = new ProductModel();

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProductUrl = Convert.ToString(rdr["ProductUrl"]);
                        prod.ProductPhoto = Convert.ToString(rdr["PhotoPath"]);
                        if (!DBNull.Value.Equals(rdr["ActualPrice"]))
                            prod.ActualPrice = Convert.ToDecimal(rdr["ActualPrice"]);

                    }
                }

                return prod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<T_ProductPeriods> GetPeriods(int productId)
        {
            try
            {
                IList<T_ProductPeriods> lstPeriod = new List<T_ProductPeriods>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProdProgress", strConn);
                cmd.Parameters["@p_productId"].Value = productId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_ProductPeriods period = new T_ProductPeriods();
                        period.PeriodID = Convert.ToInt32(rdr["PeriodID"]);
                        //period.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        period.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        period.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        period.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        period.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        period.ProductPrice = Convert.ToInt32(rdr["ProductPrice"]);
                        lstPeriod.Add(period);
                    }
                }

                return lstPeriod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<T_Photo> GetImageByType(int type, int periodId)
        {
            try
            {
                IList<T_Photo> lstPhotos = new List<T_Photo>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetImagesByType", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_type"].Value = type;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_Photo period = new T_Photo();
                        period.RelId = Convert.ToInt32(rdr["RelId"]);
                        period.PhotoPath = Convert.ToString(rdr["PhotoPath"]);
                        period.PhotoType = Convert.ToInt32(rdr["PhotoType"]);
                        period.PhotoID = Convert.ToInt32(rdr["PhotoID"]);

                        lstPhotos.Add(period);
                    }
                }

                return lstPhotos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //查询产品某期评论点赞数量
        public BaseResult GetRelPeriodInfo(int periodId,out int suppNum,out int commNum)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetRelInfoByPeriod", strConn);
                cmd.Parameters["@o_suppNum"].Value = 0;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@o_commNum"].Value = 0;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                suppNum = (int)cmd.Parameters["@o_suppNum"].Value;
                commNum = (int)cmd.Parameters["@o_commNum"].Value;
                br.Succeeded = true;
                
            }
            catch (Exception ex)
            {
                suppNum = 0;
                commNum = 0;
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

        public IList<UserOrderDTO> GetOrderList(int periodId)
        {
            try
            {
                IList<UserOrderDTO> lstOrder = new List<UserOrderDTO>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetOrderList", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
               
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        UserOrderDTO order = new UserOrderDTO();
                        order.PeriodID = Convert.ToInt32(rdr["PeriodID"]);
                        order.City = Convert.ToString(rdr["City"]);
                        order.UserName = Convert.ToString(rdr["UserName"]);
                        order.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        order.PhotoPath = Convert.ToString(rdr["PhotoPath"]);
                        if(!DBNull.Value.Equals(rdr["BuyNum"]))
                            order.BuyNum = Convert.ToInt32(rdr["BuyNum"]);
                        if (!DBNull.Value.Equals(rdr["CreateTime"]))
                            order.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);

                        lstOrder.Add(order);
                    }
                }

                return lstOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //赠送人记录
        public IList<DonateModel> GetDonaterList(int periodId)
        {
            try
            {
                IList<DonateModel> lstDonate = new List<DonateModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetUserDonate", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        DonateModel donate = new DonateModel();
                        donate.PeriodID = Convert.ToInt32(rdr["PeriodID"]);
                        donate.City = Convert.ToString(rdr["City"]);
                        donate.UserName = Convert.ToString(rdr["UserName"]);
                        donate.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        donate.PhotoPath = Convert.ToString(rdr["PhotoPath"]);
                        donate.ShareNum = Convert.ToInt32(rdr["ShareNum"]);

                        lstDonate.Add(donate);
                    }
                }

                return lstDonate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //产品列表
        public IList<ProductModel> GetEnshrineProducts(int  userId = 0)
        {
            try
            {
                IList<ProductModel> lstProd = new List<ProductModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetEnshrineProducts", strConn);
                cmd.Parameters["@p_UserId"].Value = userId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        ProductModel prod = new ProductModel();
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        //prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.UserName = Convert.ToString(rdr["UserName"]);
                        lstProd.Add(prod);
                    }
                }

                return lstProd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //确认中奖号码
       public UserOrderDTO ConfirmLottery(string lotteryTicket, int periodId)
       {
            try
            {
                UserOrderDTO userOrder = new UserOrderDTO();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pConfirmLottery", strConn);
                cmd.Parameters["@p_lotteryTicket"].Value = lotteryTicket;
                cmd.Parameters["@p_periodId"].Value = periodId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                       
                        userOrder.PeriodID= Convert.ToInt32(rdr["PeriodID"]);
                        userOrder.LotteryNum = Convert.ToString(rdr["LotteryNum"]);
                    }
                }

                return userOrder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //分享礼物
        public BaseResult AddGift(T_User_Share shareDto)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddGift", strConn);
                cmd.Parameters["@p_userId"].Value = shareDto.ShareUserId;
                cmd.Parameters["@p_periodId"].Value = shareDto.PeriodId;
                cmd.Parameters["@p_IsPay"].Value = shareDto.IsPay;
                cmd.Parameters["@p_PeopleNum"].Value = shareDto.PeopleNum;
                cmd.Parameters["@p_ShareNum"].Value = shareDto.ShareNum;
                cmd.Parameters["@p_UsedYE"].Value = shareDto.UsedYE;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;
                br.ResultId = cmd.Parameters["@o_shareId"].Value.ToString();
            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }
        //更改礼物支付状态
        public BaseResult UpdGiftSts(int shareId, int sts)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pUpdShareSts", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;
                cmd.Parameters["@p_sts"].Value = sts;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

       public T_User_Share GetShareById(int shareId)
       {
           T_User_Share shareDto = new T_User_Share();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetShareById", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {

                        shareDto.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        shareDto.PeopleNum = Convert.ToInt32(rdr["PeopleNum"]);
                        shareDto.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        shareDto.ShareUserId = Convert.ToInt32(rdr["ShareUserId"]);
                        shareDto.ShareNum = Convert.ToInt32(rdr["ShareNum"]);
                        shareDto.RevPeopleNum = Convert.ToInt32(rdr["RevPeopleNum"]);
                        shareDto.RevGiftNum = Convert.ToInt32(rdr["RevGiftNum"]);
                        shareDto.PhotoPath = Convert.ToString(rdr["PhotoPath"]);
                        shareDto.Winner = Convert.ToString(rdr["Winner"]);
                        shareDto.WinPhoto = Convert.ToString(rdr["WinPhoto"]);
                    }
                }
            }
            catch (Exception ex)
            {
                shareDto = new T_User_Share();
            }
            return shareDto;
        }

       public bool isRevGift(int shareId, int userId)
       {
            bool flag = true;
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pIsRevGift", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;
                cmd.Parameters["@p_userId"].Value = userId;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);
                int count =Convert.ToInt32(cmd.Parameters["@o_count"].Value);
                if (count == 0)
                    flag = false;
            }
            catch (Exception ex)
            {
                flag = true;
            }
           return flag;
       }

        public BaseResult UpdRevGiftInfo(int shareId, int userId,int RevNum,int periodId,string lotteryNO)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pUpdRevGiftInfo", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_RevNum"].Value = RevNum;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_LotteryNO"].Value = lotteryNO;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

      

        public BaseResult AddProdStock(int prodType, string prodName, decimal stockPrice, int stockNum,string productUrl, string prodDesc)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddProdStock", strConn);
                cmd.Parameters["@p_prodType"].Value = prodType;
                cmd.Parameters["@p_prodName"].Value = prodName;
                cmd.Parameters["@p_stockPrice"].Value = stockPrice;
                cmd.Parameters["@p_stockNum"].Value = stockNum;
                cmd.Parameters["@p_productUrl"].Value = productUrl;
                cmd.Parameters["@p_prodDesc"].Value = prodDesc;
                cmd.Parameters["@o_prodId"].Value = 0;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;
                br.ResultId = cmd.Parameters["@o_prodId"].Value.ToString();
            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

       public List<T_Product> GetAllProds()
       {
           List<T_Product> lstProd = new List<T_Product>();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetAllProd", strConn);
                
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_Product prod = new T_Product();

                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.IsActual = Convert.ToInt32(rdr["IsActual"]);
                        prod.ProductUrl = Convert.ToString(rdr["ProductUrl"]);
                        prod.ProductNO = Convert.ToString(rdr["ProductNO"]);
                        prod.InventoryNum = Convert.ToInt32(rdr["InventoryNum"]);

                        lstProd.Add(prod);
                    }
                }

            }
            catch (Exception ex)
            {
                lstProd = new List<T_Product>();
            }

           return lstProd;
       }

       public BaseResult PublishProdData(decimal totalPrice, int unitPrice, int needNum, decimal prodAC, int prodType, int prodId)
       {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pPublishProdData", strConn);
                cmd.Parameters["@p_totalPrice"].Value = totalPrice;
                cmd.Parameters["@p_unitPrice"].Value = unitPrice;
                cmd.Parameters["@p_needNum"].Value = needNum;
                cmd.Parameters["@p_prodAC"].Value = prodAC;
                cmd.Parameters["@p_prodType"].Value = prodType;
                cmd.Parameters["@p_prodId"].Value = prodId;
                
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

        //得到所有产品及期数数据
        public List<T_Product> GetAllProdPeriods()
        {
            List<T_Product> lstProd = new List<T_Product>();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetAllProd", strConn);

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_Product prod = new T_Product();

                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductName = Convert.ToString(rdr["ProductName"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.IsActual = Convert.ToInt32(rdr["IsActual"]);
                        prod.ProductUrl = Convert.ToString(rdr["ProductUrl"]);
                        prod.ProductNO = Convert.ToString(rdr["ProductNO"]);
                        prod.InventoryNum = Convert.ToInt32(rdr["InventoryNum"]);

                        prod.prodPeriods = GetAllPeriodsById(prod.ProductId);
                        lstProd.Add(prod);
                    }
                }

            }
            catch (Exception ex)
            {
                lstProd = new List<T_Product>();
            }

            return lstProd;
        }

       public List<T_ProductPeriods> GetAllPeriodsById(int productId)
       {
            List<T_ProductPeriods> lstProd = new List<T_ProductPeriods>();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetAllPriodById", strConn);
                cmd.Parameters["@p_productId"].Value = productId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_ProductPeriods prod = new T_ProductPeriods();

                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToInt32(rdr["ProductPrice"]);
                        prod.UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                        prod.ProLotteryNum = Convert.ToString(rdr["ProLotteryNum"]);
                        prod.PeriodID = Convert.ToInt32(rdr["PeriodID"]);

                        lstProd.Add(prod);
                    }
                }

            }
            catch (Exception ex)
            {
                lstProd = new List<T_ProductPeriods>();
            }

            return lstProd;
        }

        public BaseResult OpenLot(int periodId, string lotteryNo)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pConfirmLottery", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_lotteryTicket"].Value = lotteryNo;
                
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

       public List<T_Share_Get> GetRevGiftByShareId(int shareId)
       {
            List<T_Share_Get> lstShareDto = new List<T_Share_Get>();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetRevGiftById", strConn);
                cmd.Parameters["@p_shareId"].Value = shareId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_Share_Get shareDto = new T_Share_Get();
                        shareDto.GetNum = Convert.ToInt32(rdr["GetNum"]);
                        shareDto.GetUserId = Convert.ToInt32(rdr["GetUserId"]);
                        shareDto.ShareId = Convert.ToInt32(rdr["ShareId"]);
                        shareDto.UserName = Convert.ToString(rdr["UserName"]);
                        if (!DBNull.Value.Equals(rdr["RevTime"]))
                             shareDto.RevTime = Convert.ToDateTime(rdr["RevTime"]);
                        shareDto.LotNum = Convert.ToString(rdr["LotNum"]);
                        shareDto.PhotoPath = Convert.ToString(rdr["PhotoPath"]);

                        lstShareDto.Add(shareDto);
                    }
                }

            }
            catch (Exception ex)
            {
                lstShareDto = new List<T_Share_Get>();
            }
            return lstShareDto;
        }

       #region 照片
        public BaseResult AddPic(int periodId, string picPath,int picType)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddPic", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_picPath"].Value = picPath;
                cmd.Parameters["@p_picType"].Value = picType;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }
        #endregion

        #region X用户操作
        //后台用户购买操作
        public BaseResult AddLotByXUser(int userId, int addNum, int periodId, string lotteryNO)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddLotByXUser", strConn);
               cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_RevNum"].Value = addNum;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_LotteryNO"].Value = lotteryNO;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;

            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }
        #endregion
    }

}
