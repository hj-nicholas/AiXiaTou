using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 扫描二维码消息
    /// </summary>
    public class RequestScanMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取二维码的scene_id
        /// </summary>
        public uint EventKey { get; private set; }
        /// <summary>
        /// 获取二维码的ticket
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
        public RequestScanMessage(string toUserName, string fromUserName, DateTime createTime,
            uint eventKey, string ticket)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.SCAN)
        {
            EventKey = eventKey;
            Ticket = ticket;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n二维码的scene_id：{1}\r\n二维码的ticket：{2}",
                base.ToString(), EventKey, Ticket);
        }
    }
}