using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class RequestImageMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取图片链接
        /// </summary>
        public string PicUrl { get; private set; }
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
        /// <param name="picUrl"></param>
        /// <param name="mediaId"></param>
        /// <param name="msgId"></param>
        public RequestImageMessage(string toUserName, string fromUserName, DateTime createTime,
            string picUrl, string mediaId, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.image)
        {
            PicUrl = picUrl;
            MediaId = mediaId;
            MsgId = msgId;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n图片链接：{1}\r\n媒体ID：{2}\r\n消息ID：{3}",
                base.ToString(), PicUrl, MediaId, MsgId);
        }
    }
}