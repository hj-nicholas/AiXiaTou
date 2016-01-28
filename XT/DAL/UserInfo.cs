using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common;
using Model;

namespace DAL
{
    public class UserInfo
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

        public UserDTO GetUserInfo(int userId)
        {
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetUserInfo", strConn);
                cmd.Parameters["@p_UserId"].Value = userId;
                UserDTO userInfo = new UserDTO();

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        userInfo.UserID = Convert.ToInt32(rdr["UserID"]);
                        userInfo.UserName = Convert.ToString(rdr["UserName"]);
                        userInfo.AccountBalance = Convert.ToInt32(rdr["AccountBalance"]);
                        userInfo.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        userInfo.DonateNum = Convert.ToInt32(rdr["DonateNum"]);
                        userInfo.Cellphone = Convert.ToString(rdr["Cellphone"]);
                        userInfo.PhotoPath= Convert.ToString(rdr["PhotoPath"]);
                    }
                }

                    return userInfo;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public IList<T_UserAddr> GetReceiveAddrs(int userId)
        {
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetReceiveAddrs", strConn);
                cmd.Parameters["@p_UserId"].Value = userId;
                List<T_UserAddr> lstAddrs = new List<T_UserAddr>();

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_UserAddr addr = new T_UserAddr();
                        addr.UserID = Convert.ToInt32(rdr["UserID"]);
                        addr.AddressId = Convert.ToInt32(rdr["AddressId"]);
                        addr.AddressDesc = Convert.ToString(rdr["AddressDesc"]);
                        addr.Receiver = Convert.ToString(rdr["Receiver"]);
                        addr.Phone = Convert.ToString(rdr["Phone"]);
                        addr.PostCode = Convert.ToString(rdr["PostCode"]);

                        lstAddrs.Add(addr);
                    }

                }

                return lstAddrs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserDTO UpdOrInsertUser(UserDTO userDto)
        {
            try
            {
                UserDTO user = new UserDTO();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pUpdOrInsertUser", strConn);
                cmd.Parameters["@p_OpenId"].Value = userDto.OpenId;
                cmd.Parameters["@p_WeChatName"].Value = userDto.WeChatName;
                cmd.Parameters["@p_City"].Value = userDto.City;
                cmd.Parameters["@p_PhotoPath"].Value = userDto.PhotoPath;
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    
                    while (rdr.Read())
                    {
                        user.UserID = Convert.ToInt32(rdr["UserId"]);
                        user.UserName = Convert.ToString(rdr["UserName"]);
                        user.AccountBalance = Convert.ToInt32(rdr["AccountBalance"]);
                        user.PhotoPath = Convert.ToString(rdr["PhotoPath"]);
                        user.City = Convert.ToString(rdr["City"]);
                        user.OpenId = Convert.ToString(rdr["OpenId"]);
                    }

                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BaseResult EditAddr(T_UserAddr addr, int type)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pEditAddress", strConn);
                cmd.Parameters["@p_AddressDesc"].Value = addr.AddressDesc;
                cmd.Parameters["@p_AddressId"].Value = addr.AddressId;
                cmd.Parameters["@p_Phone"].Value = addr.Phone;
                cmd.Parameters["@p_PostCode"].Value = addr.PostCode;
                cmd.Parameters["@p_Receiver"].Value = addr.Receiver;
                cmd.Parameters["@p_UserID"].Value = addr.UserID;
                cmd.Parameters["@p_type"].Value = type;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        //根据用户id查询账户信息
        public List<T_Account> GetAccountByUserId(int userId)
        {
            List<T_Account> lstAcc = new List<T_Account>();
            SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetAccountByUserId", strConn);
            cmd.Parameters["@p_userId"].Value = userId;
            try
            {
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {

                    while (rdr.Read())
                    {
                        T_Account acc = new T_Account();
                        acc.UserId = Convert.ToInt32(rdr["UserId"]);
                        acc.AccountId = Convert.ToInt32(rdr["AccountId"]);
                        acc.AccountDesc = Convert.ToString(rdr["AccountDesc"]);
                        acc.AccountYE = Convert.ToInt32(rdr["AccountYE"]);
                        acc.ConsumeTime = Convert.ToDateTime(rdr["ConsumeTime"]);
                        acc.CosteAmount = Convert.ToInt32(rdr["CosteAmount"]);

                        lstAcc.Add(acc);
                    }

                }
            }
            catch(Exception ex)
            {
                lstAcc = new List<T_Account>();
                throw ex;
            }
            return lstAcc;
           
        }

        public BaseResult RechargeAcc(int userId,int chargeNum)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pRechargeAcc", strConn);
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_chargeNum"].Value = chargeNum;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        //用户送出礼物列表
        public List<T_User_Share> GetSendGifyByUserId(int userId)
        {
            List<T_User_Share> lstShare = new List<T_User_Share>();
            SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetSentGift", strConn);
            cmd.Parameters["@p_userId"].Value = userId;
            try
            {
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {

                    while (rdr.Read())
                    {
                        T_User_Share share = new T_User_Share();

                        share.ShareId = Convert.ToInt32(rdr["ShareId"]);
                        share.ShareUserId = Convert.ToInt32(rdr["ShareUserId"]);
                        share.ShareNum = Convert.ToInt32(rdr["ShareNum"]);
                        share.RevGiftNum = Convert.ToInt32(rdr["RevGiftNum"]);
                        share.ProductName = Convert.ToString(rdr["ProductName"]);
                        share.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        share.LotteryNum = Convert.ToString(rdr["LotteryNum"]);
                        share.Winner = Convert.ToString(rdr["Winner"]);
                        share.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        if(!DBNull.Value.Equals(rdr["ActualPrice"]))
                            share.ActualPrice = Convert.ToDecimal(rdr["ActualPrice"]);

                        lstShare.Add(share);
                    }

                }
            }
            catch (Exception ex)
            {
                lstShare = new List<T_User_Share>();
                throw ex;
            }
            return lstShare;

        }

        public List<T_User_Share> GetRevGifyByUserId(int userId)
        {
            List<T_User_Share> lstShare = new List<T_User_Share>();
            SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetRevGift", strConn);
            cmd.Parameters["@p_userId"].Value = userId;
            try
            {
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {

                    while (rdr.Read())
                    {
                        T_User_Share share = new T_User_Share();

                        share.ShareId = Convert.ToInt32(rdr["ShareId"]);
                        share.ShareUserId = Convert.ToInt32(rdr["ShareUserId"]);
                        share.ShareNum = Convert.ToInt32(rdr["ShareNum"]);
                        share.RevGiftNum = Convert.ToInt32(rdr["RevGiftNum"]);
                        share.ProductName = Convert.ToString(rdr["ProductName"]);
                        share.CreateTime = Convert.ToDateTime(rdr["CreateTime"]);
                        share.LotteryNum = Convert.ToString(rdr["LotteryNum"]);
                        share.Winner = Convert.ToString(rdr["Winner"]);
                        share.ProductPrice = Convert.ToDecimal(rdr["ProductPrice"]);
                        share.SendUser = Convert.ToString(rdr["SendUser"]);
                        share.GetLotNum = Convert.ToString(rdr["GetLotNum"]);
                        if (!DBNull.Value.Equals(rdr["ActualPrice"]))
                            share.ActualPrice = Convert.ToDecimal(rdr["ActualPrice"]);
                        if (!DBNull.Value.Equals(rdr["WinUserId"]))
                            share.WinUserId = Convert.ToInt32(rdr["WinUserId"]);

                        lstShare.Add(share);
                    }

                }
            }
            catch (Exception ex)
            {
                lstShare = new List<T_User_Share>();
                throw ex;
            }
            return lstShare;

        }

        public BaseResult AddShow(string content,int periodId,int userId)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddShowOrder", strConn);
                cmd.Parameters["@p_content"].Value = content;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_userId"].Value = userId;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        //用户分享，领取虾仔
        public BaseResult RevShrimp(int shareUserId, int revUserId)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pRevShrimp", strConn);
                cmd.Parameters["@p_shareUserId"].Value = shareUserId;
                cmd.Parameters["@p_revUserId"].Value = revUserId;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        public List<T_Red_Envelope> GetRedEnvelopeByUser(int userId)
        {
            List<T_Red_Envelope> lstEnv = new List<T_Red_Envelope>();
            SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetRedEnvelopeByUser", strConn);
            cmd.Parameters["@p_userId"].Value = userId;
            try
            {
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {

                    while (rdr.Read())
                    {
                        T_Red_Envelope env = new T_Red_Envelope();
                        env.DonateUserId = Convert.ToInt32(rdr["DonateUserId"]);
                        env.RevTime = Convert.ToDateTime(rdr["RevTime"]);
                        env.RevUserName = Convert.ToString(rdr["RevUserName"]);
                        env.RevUserPhoto = Convert.ToString(rdr["RevUserPhoto"]);
                        env.DonateUserId = Convert.ToInt32(rdr["DonateUserId"]);

                        lstEnv.Add(env);
                    }

                }
            }
            catch (Exception ex)
            {
                lstEnv = new List<T_Red_Envelope>();
                throw ex;
            }
            return lstEnv;
        }

        //保存奖品
        public BaseResult SaveAward(int userId, int periodId, int type, string awardNo, int addrId)
        {
            BaseResult result = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pSaveAward", strConn);
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_type"].Value = type;
                cmd.Parameters["@p_awardNo"].Value = awardNo;
                cmd.Parameters["@p_addrId"].Value = addrId;
                
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        public List<UserDTO> GetUserByType(int type)
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetUserInfo", strConn);
                cmd.Parameters["@p_type"].Value = type;
                
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        UserDTO userInfo = new UserDTO();
                        userInfo.UserID = Convert.ToInt32(rdr["UserID"]);
                        userInfo.UserName = Convert.ToString(rdr["UserName"]);
                        userInfo.AccountBalance = Convert.ToInt32(rdr["AccountBalance"]);
                        userInfo.JoinedNum = Convert.ToInt32(rdr["JoinedNum"]);
                        userInfo.DonateNum = Convert.ToInt32(rdr["DonateNum"]);
                        userInfo.Cellphone = Convert.ToString(rdr["Cellphone"]);
                        userInfo.PhotoPath = Convert.ToString(rdr["PhotoPath"]);

                        lstUser.Add(userInfo);
                    }
                }

                return lstUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BaseResult UpdUserAcc(int accNum, int userId)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pUpdUserAcc", strConn);
                cmd.Parameters["@p_AccNum"].Value = accNum;
                cmd.Parameters["@p_userId"].Value = userId;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        public T_UserAddr GetAwardAddr(int periodId)
        {
            T_UserAddr addr = new T_UserAddr();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetAwardAddrById", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;

                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        addr.UserID = Convert.ToInt32(rdr["UserID"]);
                        addr.AddressId = Convert.ToInt32(rdr["AddressId"]);
                        addr.AddressDesc = Convert.ToString(rdr["AddressDesc"]);
                        addr.Receiver = Convert.ToString(rdr["Receiver"]);
                        addr.Phone = Convert.ToString(rdr["Phone"]);
                        addr.PostCode = Convert.ToString(rdr["PostCode"]);
                      
                    }
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addr;
        }
        
            public BaseResult AddAwardInfo(int periodId, string awardNo)
        {
            BaseResult result = new BaseResult();
            try
            {

                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddAwardInfo", strConn);
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_awardNo"].Value = awardNo;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                result.Succeeded = true;
            }

            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
    }
}

