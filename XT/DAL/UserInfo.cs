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
    }
}
