using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 订阅消息
    /// </summary>
    public class RequestSubscribeMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取二维码的参数值（仅当用户扫描二维码订阅时，有二维码参数；否则为null。）
        /// </summary>
        public string EventKey { get; private set; }
        /// <summary>
        /// 获取二维码的ticket（仅当用户扫描二维码订阅时，有二维码ticket；否则为null。）
        /// </summary>
        public string Ticket { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        /// <param name="ticket"></param>
        public RequestSubscribeMessage(string toUserName, string fromUserName, DateTime createTime,
            string eventKey,string ticket)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.subscribe)
        {
            EventKey = eventKey;
            Ticket = ticket;
        }

        public RequestSubscribeMessage(string toUserName, string fromUserName, DateTime createTime)
            : this(toUserName, fromUserName, createTime, null, null)
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n二维码的参数值：{1}\r\n二维码的ticket：{2}",
                base.ToString(), EventKey ?? "", Ticket ?? "");
        }
    }
}