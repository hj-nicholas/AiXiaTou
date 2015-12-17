using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 公众号信息
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// 原始公众号（注意：不是登陆账号）
        /// </summary>
        private string userName;
        /// <summary>
        /// 应用ID
        /// </summary>
        private string appId;
        /// <summary>
        /// 应用密匙
        /// </summary>
        private string appSecret;
        /// <summary>
        /// 令牌
        /// </summary>
        private string token;

        /// <summary>
        /// 原始公众号（注意：不是登陆账号）
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("UserName", "原始公众号不能为空。");
                userName = value;
            }
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId
        {
            get
            {
                return appId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("AppId", "应用ID不能为空。");
                appId = value;
            }
        }
        /// <summary>
        /// 应用密匙
        /// </summary>
        public string AppSecret
        {
            get
            {
                return appSecret;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("AppSecret", "应用密匙不能为空。");
                appSecret = value;
            }
        }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Token", "令牌不能为空。");
                token = value;
            }
        }
        /// <summary>
        /// 消息AES加密密匙
        /// </summary>
        public string EncodingAESKey { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName">原始公众号</param>
        /// <param name="appId">应用id</param>
        /// <param name="appSecret">应用密匙</param>
        /// <param name="token">令牌</param>
        /// <param name="encodingAesKey">消息AES加密密匙</param>
        public AccountInfo(string userName, string appId, string appSecret, string token, string encodingAesKey)
        {
            UserName = userName;
            AppId = appId;
            AppSecret = appSecret;
            Token = token;
            EncodingAESKey = encodingAesKey;
            Caption = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName">原始公众号</param>
        /// <param name="appId">应用id</param>
        /// <param name="appSecret">应用密匙</param>
        /// <param name="token">令牌</param>
        /// <param name="encodingAesKey">消息AES加密密匙</param>
        /// <param name="caption">标题</param>
        public AccountInfo(string userName, string appId, string appSecret, string token, string encodingAesKey, string caption)
            : this(userName, appId, appSecret, token, encodingAesKey)
        {
            Caption = caption;
        }
    }
}