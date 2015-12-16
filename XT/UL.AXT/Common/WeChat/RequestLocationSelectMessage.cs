using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 弹出地理位置选择器的事件推送消息
    /// </summary>
    public class RequestLocationSelectMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取事件KEY值
        /// </summary>
        public string EventKey { get; private set; }
        /// <summary>
        /// 发送的位置信息
        /// </summary>
        public SendLocationInfo SendLocationInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        /// <param name="sendLocationInfo"></param>
        public RequestLocationSelectMessage(string toUserName, string fromUserName, DateTime createTime,
            string eventKey, SendLocationInfo sendLocationInfo)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.location_select)
        {
            SendLocationInfo = sendLocationInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventKey"></param>
        /// <param name="locationX"></param>
        /// <param name="locationY"></param>
        /// <param name="scale"></param>
        /// <param name="label"></param>
        /// <param name="poiname"></param>
        public RequestLocationSelectMessage(string toUserName, string fromUserName, DateTime createTime,
            string eventKey, double locationX, double locationY, int scale, string label, string poiname = null)
            : this(toUserName, fromUserName, createTime, eventKey, new SendLocationInfo(locationX, locationY, scale, label, poiname))
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n事件KEY值：{1}\r\n发送的位置信息：{2}",
                base.ToString(), EventKey, SendLocationInfo);
        }
    }
}
