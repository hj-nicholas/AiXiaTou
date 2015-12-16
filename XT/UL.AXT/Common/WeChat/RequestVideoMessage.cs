using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class RequestVideoMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取视频缩略图媒体ID
        /// </summary>
        public string ThumbMediaId { get; private set; }
        /// <summary>
        /// 获取媒体ID
        /// </summary>
        public string MediaId { get; private set; }
        /// <summary>
        /// 获取消息ID
        /// </summary>
        public long MsgId { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="thumbMediaId"></param>
        /// <param name="mediaId"></param>
        /// <param name="msgId"></param>
        public RequestVideoMessage(string toUserName, string fromUserName, DateTime createTime,
            string thumbMediaId, string mediaId, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.video)
        {
            ThumbMediaId = thumbMediaId;
            MediaId = mediaId;
            MsgId = msgId;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n视频缩略图媒体ID：{1}\r\n媒体ID：{2}\r\n消息ID：{3}",
                base.ToString(), ThumbMediaId, MediaId, MsgId);
        }
    }
}