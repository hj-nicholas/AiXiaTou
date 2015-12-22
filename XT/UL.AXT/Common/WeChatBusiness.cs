using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Hoo.Common.WeChat;
using Model;

namespace UL.AXT.Common
{
    public class WeChatBusiness
    {
        public static readonly string PublicAccount = WebConfigurationManager.AppSettings["PublicAccount"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string DefaultToken = WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string AppSecret = WebConfigurationManager.AppSettings["AppSecret"];//与微信公众账号后台的AppId设置保持一致，区分大小写。

        private BLL.UserInfo userInfo = new BLL.UserInfo();

        //用户登录后获取用户信息
        public UserInfo GetWeChatUser(string code)
        {
            //添加微信公众账号
            AccountInfo account = AccountInfoCollection.GetAccountInfo(PublicAccount);
            if (account == null)
                AddPublicAccount();

            //1 第一步：用户同意授权，获取code
            //2 第二步：通过code换取网页授权access_token
            //3 第三步：刷新access_token（如果需要）
            //4 第四步：拉取用户信息(需scope为 snsapi_userinfo)
            //5 附：检验授权凭证（access_token）是否有效

            UserInfo user = null;
            ErrorMessage errMsg = null;
            OAuthAccessToken token = OAuthAccessToken.Get(PublicAccount, code, out errMsg);

            //OAuthAccessToken token = OAuthAccessToken.Refresh(PublicAccount, token.refresh_token, errMsg);

            if (token != null)
                user = OAuthAccessToken.GetUserInfo(token.access_token, token.openid, out errMsg);

            return user;

        }

        public void AddPublicAccount()
        {
            AccountInfo account = new AccountInfo(PublicAccount, AppId, AppSecret, DefaultToken, EncodingAESKey);
            AccountInfoCollection.SetAccountInfo(account);

        }

        public UserDTO ChangeUserByWeChatInfo(UserInfo user)
        {
            UserDTO newUserDto = new UserDTO();
            if (user != null)
            {
                UserDTO userDto = new UserDTO();
                //userDto.WeChatName = user.nickname;
                //userDto.PhotoPath = user.headimgurl;
                //userDto.City = user.city;
                //userDto.OpenId = user.openid;
                userDto.OpenId = "orIUqxN88RpM6MLpS8K45cM8qNOc";
                userDto.WeChatName = "$天门琴痴$";
                userDto.PhotoPath = "http://wx.qlogo.cn/mmopen/ajNVdqHZLLCR4ZVgDNqFmrpJvmtLkpVg0jU5etngPK98SohUpriaWJtR2Mma4gdnSmolsf9SkY4oVoic8SyQ7BwA/0";
                userDto.City = "ShenZhen";
                //更新用户信息并查询出该用户ID
                newUserDto = userInfo.UpdOrInsertUser(userDto);
            }

            return newUserDto;
        }
    }
}
