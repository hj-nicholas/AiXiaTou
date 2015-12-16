using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 客服接入会话事件消息
    /// </summary>
    public class RequestKfCreateSessionMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取客服账号
        /// </summary>
        public string KfAccount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="kfAccount">客服账号</param>
        public RequestKfCreateSessionMessage(string toUserName, string fromUserName, DateTime createTime,
            string kfAccount)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.kf_create_session)
        {
            KfAccount = kfAccount;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n客服账号：{1}",
                base.ToString(), KfAccount);
        }
    }
}