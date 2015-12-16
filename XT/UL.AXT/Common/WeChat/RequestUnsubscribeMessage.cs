using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 取消订阅消息
    /// </summary>
    public class RequestUnsubscribeMessage : RequestEventMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        public RequestUnsubscribeMessage(string toUserName, string fromUserName, DateTime createTime)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.unsubscribe)
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}