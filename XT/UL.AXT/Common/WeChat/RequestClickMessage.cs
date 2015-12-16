using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 点击菜单消息
    /// </summary>
    public class RequestClickMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取菜单的KEY值
        /// </summary>
        public string EventKey { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        public RequestClickMessage(string toUserName, string fromUserName, DateTime createTime,
            string eventKey)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.CLICK)
        {
            EventKey = eventKey;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n菜单KEY值：{1}",
                base.ToString(), EventKey);
        }
    }
}