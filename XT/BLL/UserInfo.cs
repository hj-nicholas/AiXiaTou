using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class UserInfo
    {
        DAL.UserInfo user = new DAL.UserInfo();

        public UserDTO GetUserInfo(int userId)
        {
            UserDTO userInfo = user.GetUserInfo(userId);
            return userInfo;
        }

        public IList<T_UserAddr> GetReceiveAddrs(int userId)
        {
            var addrs = user.GetReceiveAddrs(userId);
            return addrs;
        }

        public UserDTO UpdOrInsertUser(UserDTO userDto)
        {
            var newUserDto = user.UpdOrInsertUser(userDto);
            return newUserDto;
        }

        public BaseResult EditAddr(T_UserAddr addr, int type)
        {
            var result = user.EditAddr(addr, type);
            return result;
        }

        public List<T_Account> GetAccountByUserId(int userId)
        {
            var lst = user.GetAccountByUserId(userId);
            return lst;
        }

        public BaseResult RechargeAcc(int userId, int chargeNum)
        {
            var result = user.RechargeAcc(userId, chargeNum);
            return result;
        }

        public List<T_User_Share> GetSendGifyByUserId(int userId)
        {
            var lst = user.GetSendGifyByUserId(userId);
            return lst;
        }

        public List<T_User_Share> GetRevGifyByUserId(int userId)
        {
            var lst = user.GetRevGifyByUserId(userId);
            return lst;
        }

        public BaseResult RevShrimp(int shareUserId, int revUserId)
        {

            var result = user.RevShrimp(shareUserId, revUserId);
            return result;
        }

        public List<T_Red_Envelope> GetRedEnvelopeByUser(int userId)
        {
            var lst = user.GetRedEnvelopeByUser(userId);
            return lst;
        }

        public BaseResult SaveAward(int userId, int periodId, int type, string awardNo, int addrId)
        {
            var result = user.SaveAward(userId, periodId,type,awardNo,addrId);
            return result;
        }

    }
}
