using System;
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
       public IList<ShowOrderModel> GetShowingOrders()
       {
            try
            {
                IList<ShowOrderModel> lstShowOrder = new List<ShowOrderModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetShowOrder", strConn);

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
                        //showOrder.Photos = Convert.ToString(rdr["CommentContent"]);
                        showOrder.ProductName = Convert.ToString(rdr["ProductName"]);
                        showOrder.SupportNum = Convert.ToInt32(rdr["SupportNum"]);
                        showOrder.UserName = Convert.ToString(rdr["UserName"]);
                        showOrder.PeriodID = Convert.ToInt32(rdr["PeriodId"]);
                        showOrder.UserImage = Convert.ToString(rdr["PhotoPath"]);

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
        public IList<ProductModel> GetProducts(int proType)
        {
            try
            {
                IList<ProductModel> lstProd = new List<ProductModel>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetProducts", strConn);
                cmd.Parameters["@p_ProType"].Value = proType;

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

    }
}
