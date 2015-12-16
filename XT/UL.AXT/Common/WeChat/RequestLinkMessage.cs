using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class RequestLinkMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 获取描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 获取链接
        /// </summary>
        public string Url { get; private set; }
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
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <param name="msgId"></param>
        public RequestLinkMessage(string toUserName, string fromUserName, DateTime createTime,
            string title, string description, string url, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.link)
        {
            Title = title;
            Description = description;
            Url = url;
            MsgId = msgId;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n标题：{1}\r\n描述：{2}\r\n链接：{3}\r\n消息ID：{4}",
                base.ToString(), Title, Description, Url, MsgId);
        }
    }
}