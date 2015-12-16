using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 弹出发送图片的事件推送消息，
    /// 包括：pic_sysphoto(弹出系统拍照发图),pic_photo_or_album(弹出拍照或相册发图),pic_weixin(弹出微信相册发图)
    /// </summary>
    public class RequestPicMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取事件KEY值
        /// </summary>
        public string EventKey { get; private set; }
        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfo SendPicsInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventType"></param>
        /// <param name="eventKey"></param>
        /// <param name="sendPicsInfo"></param>
        public RequestPicMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType, string eventKey, SendPicsInfo sendPicsInfo)
            : base(toUserName, fromUserName, createTime, eventType)
        {
            if (eventType != RequestEventTypeEnum.pic_sysphoto && 
                eventType != RequestEventTypeEnum.pic_photo_or_album && 
                eventType != RequestEventTypeEnum.pic_weixin)
                throw new ArgumentOutOfRangeException("Event", "错误的事件类型。");
            EventKey = eventKey;
            SendPicsInfo = sendPicsInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventType"></param>
        /// <param name="eventKey"></param>
        /// <param name="pics"></param>
        public RequestPicMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType, string eventKey, IEnumerable<string> pics)
            : this(toUserName, fromUserName, createTime, eventType, eventKey, new SendPicsInfo(pics))
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventType"></param>
        /// <param name="eventKey"></param>
        /// <param name="pic"></param>
        public RequestPicMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType, string eventKey, string pic)
            : this(toUserName, fromUserName, createTime, eventType, eventKey, new string[] { pic })
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n事件KEY值：{1}\r\n发送的图片信息：{2}",
                base.ToString(), EventKey, SendPicsInfo);
        }
    }
}
