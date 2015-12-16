using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 点击菜单跳转链接的消息
    /// </summary>
    public class RequestViewMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取链接地址
        /// </summary>
        public string EventKey { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        public RequestViewMessage(string toUserName, string fromUserName, DateTime createTime,
            string eventKey)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.VIEW)
        {
            EventKey = eventKey;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n链接地址：{1}",
                base.ToString(), EventKey);
        }
    }
}