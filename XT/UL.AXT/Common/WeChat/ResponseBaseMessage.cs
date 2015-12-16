using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 响应消息基类
    /// </summary>
    public class ResponseBaseMessage
    {
        private string toUserName;
        private string fromUserName;

        /// <summary>
        /// 获取或设置接收方帐号（收到的OpenID）
        /// </summary>
        public String ToUserName
        {
            get
            {
                return toUserName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("ToUserName", "ToUserName为空。");
                toUserName = value;
            }
        }
        /// <summary>
        /// 获取或设置开发者微信号
        /// </summary>
        public String FromUserName
        {
            get
            {
                return fromUserName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("FromUserName", "FromUserName为空");
                fromUserName = value;
            }
        }
        /// <summary>
        /// 获取或设置消息创建时间  
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public ResponseMessageTypeEnum MsgType { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="msgType">消息类型</param>
        protected ResponseBaseMessage(string toUserName, string fromUserName, DateTime createTime, ResponseMessageTypeEnum msgType)
        {
            ToUserName = toUserName;
            FromUserName = fromUserName;
            CreateTime = createTime;
            MsgType = msgType;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("接收方账号：{0}\r\n开发者微信号：{1}\r\n消息创建时间：{2}\r\n消息类型：{3:g}",
                ToUserName, FromUserName, CreateTime, MsgType);
        }

        /// <summary>
        /// 为响应消息创建XML文档
        /// </summary>
        /// <returns></returns>
        protected XmlDocument CreateXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("xml");
            doc.AppendChild(root);
            root.AppendChild(CreateXmlElement(doc, "ToUserName", ToUserName));
            root.AppendChild(CreateXmlElement(doc, "FromUserName", FromUserName));
            root.AppendChild(CreateXmlElement(doc, "CreateTime", CreateTime));
            root.AppendChild(CreateXmlElement(doc, "MsgType", MsgType));
            return doc;
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// 注：不包含数据。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <returns></returns>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name)
        {
            return doc.CreateElement(name);
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// 注：仅支持CDATA和Text类型的数据。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="data">子节点包含的数据</param>
        /// <param name="dataType">子节点数据的类型</param>
        private XmlElement CreateXmlElement(XmlDocument doc, string name, string data, XmlNodeType dataType)
        {
            XmlElement elem = doc.CreateElement(name);
            if (dataType == XmlNodeType.CDATA)
                elem.AppendChild(doc.CreateCDataSection(data));
            else if (dataType == XmlNodeType.Text)
                elem.AppendChild(doc.CreateTextNode(data));
            return elem;
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="data">子节点包含的数据</param>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name, string data)
        {
            return CreateXmlElement(doc, name, data, XmlNodeType.CDATA);
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="data">子节点包含的数据</param>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name, long data)
        {
            return CreateXmlElement(doc, name, data.ToString(), XmlNodeType.Text);
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="data">子节点包含的数据</param>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name, int data)
        {
            return CreateXmlElement(doc, name, data.ToString(), XmlNodeType.Text);
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="dt">子节点包含的数据</param>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name, DateTime data)
        {
            return CreateXmlElement(doc, name, Utility.ToWeixinTime(data).ToString(), XmlNodeType.Text);
        }

        /// <summary>
        /// 创建XmlElement节点。
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <param name="name">子节点名称</param>
        /// <param name="dt">子节点包含的数据</param>
        protected XmlElement CreateXmlElement(XmlDocument doc, string name, ResponseMessageTypeEnum data)
        {
            return CreateXmlElement(doc, name, data.ToString("g"), XmlNodeType.CDATA);
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected virtual string ToXml()
        {
            return CreateXmlDocument().InnerXml;
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <param name="encryptType">消息加密类型</param>
        /// <returns>返回XML格式的响应消息</returns>
        public string ToXml(MessageEncryptTypeEnum encryptType)
        {
            int WXBizMsgCrypt_OK = 0;
            //得到未加密的XML响应消息
            string xml = ToXml();
            //如果需要加密，加密消息
            //if (encryptType == MessageEncryptTypeEnum.aes)
            //{
            //    int timeStamp = Utility.ToWeixinTime(CreateTime);
            //    Random random = new Random();
            //    string nonce = random.Next().ToString();
            //    AccountInfo account = AccountInfoCollection.GetAccountInfo(FromUserName);
            //    if (account != null)
            //    {
            //        Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(account.Token, account.EncodingAESKey, account.AppId);
            //        string xmlEncrypt = "";
            //        //加密消息
            //        if (wxcpt.EncryptMsg(xml, timeStamp.ToString(), nonce, ref xmlEncrypt) == WXBizMsgCrypt_OK)
            //            return xmlEncrypt;
            //    }
            //}
            return xml;
        }

        /// <summary>
        /// 返回JSON格式的客服消息
        /// </summary>
        /// <returns></returns>
        public virtual string ToJson()
        {
            var customerService = new
            {
                touser = ToUserName,
                msgtype = MsgType.ToString("g")
            };
            return JsonConvert.SerializeObject(customerService);
        }
    }
}