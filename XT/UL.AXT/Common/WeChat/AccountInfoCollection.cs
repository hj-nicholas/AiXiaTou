using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 公众号集合
    /// </summary>
    public static class AccountInfoCollection
    {
        /// <summary>
        /// 已知的公众号信息
        /// </summary>
        private static ConcurrentDictionary<string, AccountInfo> accountInfos = new ConcurrentDictionary<string, AccountInfo>();

        /// <summary>
        /// 获取公众号信息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <returns>返回公众号信息；如果公众号不存在，返回null。</returns>
        public static AccountInfo GetAccountInfo(string userName)
        {
            return accountInfos.ContainsKey(userName) ? accountInfos[userName] : null;
        }

        /// <summary>
        /// 设置公众号信息
        /// </summary>
        /// <param name="account">公众号信息</param>
        public static void SetAccountInfo(AccountInfo account)
        {
            if (account == null)
                throw new ArgumentNullException("account", "公众号信息不能为空。");
            accountInfos[account.UserName] = account;
        }

        /// <summary>
        /// 获取已知的公众号
        /// </summary>
        public static ICollection<string> UserNames
        {
            get
            {
                return accountInfos.Keys;
            }
        }

        /// <summary>
        /// 获取已知的公众号信息
        /// </summary>
        public static ICollection<AccountInfo> AccountInfos
        {
            get
            {
                return accountInfos.Values;
            }
        }

        /// <summary>
        /// 获取第一个已知的公众号信息
        /// </summary>
        public static AccountInfo First
        {
            get
            {
                AccountInfo account = null;
                if (accountInfos.Count > 0)
                {
                    foreach (KeyValuePair<string, AccountInfo> pair in accountInfos)
                    {
                        account = pair.Value;
                        break;
                    }
                }
                return account;
            }
        }
    }
}
