﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
   public class Product
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        
        //晒单数据
        public IList<ShowOrderModel> GetShowingOrders(int productId,int userId=0)
       {
            try
            {
                IList<ShowOrderModel> lstShowOrder = new List<ShowOrderModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetShowOrder", strConn);
                cmd.Parameters["@p_productId"].Value = productId;
                cmd.Parameters["@p_userId"].Value = userId;

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
                        prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
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
                        prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
                        prod.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        prod.ProductId = Convert.ToInt32(rdr["ProductId"]);
                        prod.ProductType = Convert.ToInt32(rdr["ProductType"]);
                        prod.ProductDesc = Convert.ToString(rdr["ProductDesc"]);
                        prod.PeriodId = Convert.ToInt32(rdr["PeriodId"]);
                        prod.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        prod.PeriodNum = Convert.ToString(rdr["PeriodNum"]);
                        prod.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        
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
                        period.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
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
                        prod.ProductExpires = Convert.ToDateTime(rdr["ProductExpires"]);
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
    }
}
