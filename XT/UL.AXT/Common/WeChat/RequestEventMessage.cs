using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 事件推送消息
    /// </summary>
    public class RequestEventMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取事件类型
        /// </summary>
        public RequestEventTypeEnum Event { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        public RequestEventMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.Event)
        {
            Event = eventType;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n事件类型：{1:g}",
                base.ToString(), Event);
        }
    }
}