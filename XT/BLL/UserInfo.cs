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
           UserDTO userInfo= user.GetUserInfo(userId);
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
    }
}
