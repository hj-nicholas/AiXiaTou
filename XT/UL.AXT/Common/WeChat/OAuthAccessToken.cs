using System;
using System.Collections.Concurrent;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UL.AXT.Common;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 微信网页许可令牌
    /// </summary>
    public class OAuthAccessToken
    {
        /// <summary>
        /// 获取用户授权的地址
        /// </summary>
        private const string urlForGettingOAuthUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
        /// <summary>
        /// 最大state长度
        /// </summary>
        private const int maxStateLength = 128;
        /// <summary>
        /// 获取access token的地址
        /// </summary>
        private const string urlForGettingAccessToken = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        /// <summary>
        /// 刷新access token的地址
        /// </summary>
        private const string urlForRefreshingAccessToken="https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";
        /// <summary>
        /// 获取用户基本信息的地址
        /// </summary>
        private const string urlForGettingUserInfo="https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}";
        /// <summary>
        /// 校验access token有效性的地址
        /// </summary>
        private const string urlForCheckingValidate="https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}";
        /// <summary>
        /// 执行上述动作的http方法
        /// </summary>
        private const string httpMethod = WebRequestMethods.Http.Get;

        /// <summary>
        /// access token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token{get;set;}
        /// <summary>
        /// 用户id
        /// </summary>
        public string openid{get;set;}
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope{get;set;}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_access_token">access token</param>
        /// <param name="_expires_in">过期时间</param>
        /// <param name="_refresh_token">用户刷新access token</param>
        /// <param name="_openid">用户id</param>
        /// <param name="_scope">用户授权的作用域</param>
        internal OAuthAccessToken(string _access_token, int _expires_in, string _refresh_token,string _openid,string _scope)
        {
            access_token = _access_token;
            expires_in = _expires_in;
            refresh_token = _refresh_token;
            openid = _openid;
            scope = _scope;
        }

        /// <summary>
        /// 返回AccessToken字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("AccessToken：{0}\r\n过期时间：{1}秒\r\n用户刷新AccessToken：{2}\r\n用户id：{3}\r\n用户授权的作用域：{4}",
                access_token, expires_in, refresh_token, openid, scope);
        }

        /// <summary>
        /// 从JSON字符串解析AccessToken
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>返回AccessToken</returns>
        internal static OAuthAccessToken Parse(string json)
        {
            var at = JsonConvert.DeserializeAnonymousType(json,
                new { access_token = "", expires_in = 1, refresh_token = "", openid = "", scope = "" });
            return new OAuthAccessToken(at.access_token, at.expires_in, at.refresh_token, at.openid, at.scope);
        }

        /// <summary>
        /// 尝试从JSON字符串解析AccessToken
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="msg">如果解析成功，返回AccessToken；否则，返回null。</param>
        /// <returns>返回是否解析成功</returns>
        internal static bool TryParse(string json, out OAuthAccessToken token)
        {
            bool success = false;
            token = null;
            try
            {
                token = Parse(json);
                success = true;
            }
            catch { }
            return success;
        }

        /// <summary>
        /// 获取用户网页授权的地址
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="redirectUrl">用户授权之后的回调地址</param>
        /// <param name="scope">应用授权作用域</param>
        /// <param name="state">重定向之后的state参数</param>
        /// <returns>返回用户网页授权的地址；如果获取公众号信息失败或者参数错误，返回空字符串。</returns>
        public static string GetOAuthUrl(string userName, string redirectUrl, OAuthScopeEnum scope, string state = null)
        {
            string url = string.Empty;
            AccountInfo account = AccountInfoCollection.GetAccountInfo(userName);
            if (account == null)
                return url;
            if (string.IsNullOrWhiteSpace(redirectUrl))
                return url;
            if (state == null)
                state = string.Empty;
            if (state.Length > maxStateLength)
                return url;
            foreach (char c in state)
            {
                if (!char.IsLetterOrDigit(c))
                    return url;
            }
            url = string.Format(urlForGettingOAuthUrl, account.AppId, HttpUtility.UrlEncode(redirectUrl), scope.ToString("g"), state);
            return url;
        }

        /// <summary>
        /// 得到access token
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="code">用户同意授权后，得到的code</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回access token；如果获取失败，返回null。</returns>
        public static OAuthAccessToken Get(string userName, string code, out ErrorMessage errorMessage)
        {
            OAuthAccessToken token = null;
           if (string.IsNullOrWhiteSpace(code))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "用户授权后的code不能为空。");
                return token;
            }
            AccountInfo account = AccountInfoCollection.GetAccountInfo(userName);
            if (account == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取公众号信息失败。");
                return token;
            }
            Log.WriteLog("AppId", account.AppId);
            Log.WriteLog("AppSecret", account.AppSecret);
            Log.WriteLog("code", code);

            string url = string.Format(urlForGettingAccessToken, account.AppId, account.AppSecret, code);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, (string)null))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "从微信服务器获取响应失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var result = JsonConvert.DeserializeAnonymousType(responseContent,
                    new { access_token = "", expires_in = 0, refresh_token = "", openid = "", scope = "" });
                token = new OAuthAccessToken(result.access_token, result.expires_in, result.refresh_token, result.openid, result.scope);
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "获取access token成功。");
            }
            return token;
        }

        /// <summary>
        /// 刷新access token
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="refreshToken">用户刷新token</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回access token；如果获取失败，返回null。</returns>
        public static OAuthAccessToken Refresh(string userName, string refreshToken, out ErrorMessage errorMessage)
        {
            OAuthAccessToken token = null;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "用户刷新token不能为空。");
                return token;
            }
            AccountInfo account = AccountInfoCollection.GetAccountInfo(userName);
            if (account == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取公众号信息失败。");
                return token;
            }
            string url = string.Format(urlForRefreshingAccessToken, account.AppId, refreshToken);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, (string)null))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "从微信服务器获取响应失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var result = JsonConvert.DeserializeAnonymousType(responseContent,
                    new { access_token = "", expires_in = 0, refresh_token = "", openid = "", scope = "" });
                token = new OAuthAccessToken(result.access_token, result.expires_in, result.refresh_token, result.openid, result.scope);
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "获取access token成功。");
            }
            return token;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">网页access token</param>
        /// <param name="openId">用户id</param>
        /// <param name="language">语言</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetUserInfo(string accessToken, string openId, LanguageEnum language, out ErrorMessage errorMessage)
        {
            return GetUserInfo(accessToken, openId, language.ToString("g"), out errorMessage);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">网页access token</param>
        /// <param name="openId">用户id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetUserInfo(string accessToken, string openId, out ErrorMessage errorMessage)
        {
            return GetUserInfo(accessToken, openId, string.Empty, out errorMessage);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">网页access token</param>
        /// <param name="openId">用户id</param>
        /// <param name="language">语言</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        private static UserInfo GetUserInfo(string accessToken, string openId, string language, out ErrorMessage errorMessage)
        {
            UserInfo info = null;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "网页access token不能为空。");
                return info;
            }
            if (string.IsNullOrWhiteSpace(openId))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "用户id不能为空。");
                return info;
            }
            string url = string.Format(urlForGettingUserInfo, accessToken, openId, language);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, (string)null))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("openid", out jt) && (string)jt == openId)
                {
                    string nickname = jo.TryGetValue("nickname", out jt) ? (string)jt : string.Empty;
                    int sex = jo.TryGetValue("sex", out jt) ? (int)jt : (int)SexEnum.Unknown;
                    string lang = jo.TryGetValue("language", out jt) ? (string)jt : string.Empty;
                    string city = jo.TryGetValue("city", out jt) ? (string)jt : string.Empty;
                    string province = jo.TryGetValue("province", out jt) ? (string)jt : string.Empty;
                    string country = jo.TryGetValue("country", out jt) ? (string)jt : string.Empty;
                    string headimgurl = jo.TryGetValue("headimgurl", out jt) ? (string)jt : string.Empty;
                    string[] privilege = null;
                    if (jo.TryGetValue("privilege", out jt))
                    {
                        if (jt.Type == JTokenType.Array)
                        {
                            JArray ja = (JArray)jt;
                            privilege = new string[ja.Count];
                            int idx = 0;
                            foreach (JValue jv in ja)
                            {
                                privilege[idx] = (string)jv.Value;
                                idx++;
                            }
                        }
                        else if (jt.Type == JTokenType.String)
                            privilege = new string[] { (string)jt };
                    }
                    string unionid = jo.TryGetValue("unionid", out jt) ? (string)jt : string.Empty;
                    info = new UserInfo(openId, nickname, sex, lang, city, province, country, headimgurl, privilege, unionid);
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                }
                else
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取用户基本信息失败。");
            }
            return info;
        }

        /// <summary>
        /// 校验网页access token的有效性
        /// </summary>
        /// <param name="accessToken">网页许可令牌</param>
        /// <param name="openId">用户id</param>
        /// <returns>返回网页access token是否有效</returns>
        public static ErrorMessage CheckValidate(string accessToken, string openId)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "网页access token不能为空。");
            if (string.IsNullOrWhiteSpace(openId))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "用户id不能为空。");
            string url = string.Format(urlForCheckingValidate, accessToken, openId);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, (string)null))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                return ErrorMessage.Parse(responseContent);
            else
                return new ErrorMessage(ErrorMessage.ExceptionCode, "解析结果失败。");
        }
    }

    public enum OAuthScopeEnum
    {
        /// <summary>
        /// snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid）
        /// </summary>
        snsapi_base,
        /// <summary>
        /// snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。
        /// 并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
        /// </summary>
        snsapi_userinfo
    }
}