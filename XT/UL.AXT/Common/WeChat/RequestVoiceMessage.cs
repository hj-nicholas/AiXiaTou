using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class RequestVoiceMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取媒体ID
        /// </summary>
        public string MediaId { get; private set; }
        /// <summary>
        /// 获取图片链接
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 获取语音识别结果（开通语音识别后才有识别结果；否则为null）
        /// </summary>
        public string Recognition { get; private set; }
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
        /// <param name="mediaId"></param>
        /// <param name="format"></param>
        /// <param name="recognition"></param>
        /// <param name="msgId"></param>
        public RequestVoiceMessage(string toUserName, string fromUserName, DateTime createTime,
            string mediaId, string format, string recognition, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.voice)
        {
            Format = format;
            MediaId = mediaId;
            Recognition = recognition;
            MsgId = msgId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="mediaId"></param>
        /// <param name="format"></param>
        /// <param name="msgId"></param>
        public RequestVoiceMessage(string toUserName, string fromUserName, DateTime createTime,
            string mediaId, string format, long msgId)
            : this(toUserName, fromUserName, createTime, mediaId, format, null, msgId)
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n语音格式：{1}\r\n媒体ID：{2}\r\n语音识别结果：{3}\r\n消息ID：{4}",
                base.ToString(), Format, MediaId, Recognition ?? "", MsgId);
        }
    }
}