using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class RequestTextMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取消息内容
        /// </summary>
        public string Content { get; private set; }
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
        /// <param name="content"></param>
        /// <param name="msgId"></param>
        public RequestTextMessage(string toUserName, string fromUserName, DateTime createTime, 
            string content, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.text)
        {
            Content = content;
            MsgId = msgId;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n消息内容：{1}\r\n消息ID：{2}",
                base.ToString(), Content, MsgId);
        }
    }
}