using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 客服转接会话事件消息
    /// </summary>
    public class RequestKfSwitchSessionMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取会话的原客服账号
        /// </summary>
        public string FromKfAccount { get; private set; }
        /// <summary>
        /// 获取会话的新客服账号
        /// </summary>
        public string ToKfAccount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="fromKfAccount">会话的原客服账号</param>
        /// <param name="toKfAccount">会话的新客服账号</param>
        public RequestKfSwitchSessionMessage(string toUserName, string fromUserName, DateTime createTime,
            string fromKfAccount, string toKfAccount)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.kf_switch_session)
        {
            FromKfAccount = fromKfAccount;
            ToKfAccount = toKfAccount;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n原客服账号：{1}\r\n新客服账号：{2}",
                base.ToString(), FromKfAccount, ToKfAccount);
        }
    }
}