using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 响应文本消息
    /// </summary>
    public class ResponseTextMessage:ResponseBaseMessage
    {
        private string content;
        /// <summary>
        /// 获取或设置消息内容
        /// </summary>
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Content", "Content为空。");
                content = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="content">消息内容</param>
        public ResponseTextMessage(string toUserName, string fromUserName, DateTime createTime,string content): base(toUserName, fromUserName, createTime, ResponseMessageTypeEnum.text)
        {
            Content = content;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n消息内容：{1}",
                base.ToString(), Content);
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected override string ToXml()
        {
            XmlDocument doc = CreateXmlDocument();
            XmlElement root = doc.DocumentElement;
            root.AppendChild(CreateXmlElement(doc, "Content", Content));
            return doc.InnerXml;
        }

        /// <summary>
        /// 返回JSON格式的客服消息
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            var customerService = new
            {
                touser = ToUserName,
                msgtype = MsgType.ToString("g"),
                text = new
                {
                    content = Content
                }
            };
            return JsonConvert.SerializeObject(customerService);
        }
    }
}