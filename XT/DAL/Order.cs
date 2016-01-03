using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Model;

namespace DAL
{
   public class Order
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

        public BaseResult AddOrderDesc(T_User_Order_Desc order)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddOrderDesc", strConn);
                cmd.Parameters["@p_userId"].Value = order.UserId;
                cmd.Parameters["@p_periodId"].Value = order.PeriodId;
                cmd.Parameters["@p_LotteryNO"].Value = order.LotteryNO;
                cmd.Parameters["@p_OrderNO"].Value = order.OrderNO;
                cmd.Parameters["@p_UserAddr"].Value = order.UserAddr;
                cmd.Parameters["@p_UserDesc"].Value = order.UserDesc;
                cmd.Parameters["@p_UserPhone"].Value = order.UserPhone;
                cmd.Parameters["@p_OrderEmail"].Value = order.OrderEmail;
                cmd.Parameters["@p_IsPay"].Value = order.IsPay;
                cmd.Parameters["@o_orderId"].Value = 0;

                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;
                br.ResultId =cmd.Parameters["@o_orderId"].Value.ToString();
            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

       public BaseResult UpdOrderSts(int orderId, int sts)
       {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pUpdOrderSts", strConn);
                cmd.Parameters["@p_orderId"].Value = orderId;
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

       public bool IsExistsLottery(string lotteryNo, int periodId)
       {
            bool flag = false;
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pIsExistsLottery", strConn);
                cmd.Parameters["@p_lotteryNo"].Value = lotteryNo;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@o_count"].Value = 0;
                
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                if (Convert.ToUInt32(cmd.Parameters["@o_count"].Value) > 0)
                    flag= true;
                

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
    }
}
