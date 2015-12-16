using System;
using System.Collections.Generic;
using System.Xml;
using System.Web;
using System.Web.Services.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// RequestMessage：请求消息辅助类
    /// </summary>
    public class RequestMessageHelper
    {
        /// <summary>
        /// 将xml节点CDATA转换成json之后的键名
        /// </summary>
        private const string CDATA_KEY = "#cdata-section";

        /// <summary>
        /// HTTP请求
        /// </summary>
        private HttpRequest request;
        /// <summary>
        /// 请求消息
        /// </summary>
        private RequestBaseMessage message;
        /// <summary>
        /// 加密类型
        /// </summary>
        private MessageEncryptTypeEnum encryptType;

        /// <summary>
        /// 获取是否有请求消息
        /// </summary>
        public bool HasRequestMessage
        {
            get
            {
                return message != null;
            }
        }

        /// <summary>
        /// 获取消息类型
        /// </summary>
        public RequestMessageTypeEnum MessageType
        {
            get
            {
                if (message != null)
                    return message.MsgType;
                else
                    throw new NotImplementedException("请求消息为空，无法获取消息类型。");
            }
        }

        /// <summary>
        /// 获取请求消息
        /// </summary>
        public RequestBaseMessage Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// 获取消息的加密类型
        /// </summary>
        public MessageEncryptTypeEnum EncryptType
        {
            get
            {
                return encryptType;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="request">HTTP请求</param>
        public RequestMessageHelper(HttpRequest request)
        {
            if (request != null)
            {
                this.request = request;
                //获取请求体内容
                string content = Utility.GetRequestContent(request);
                //获取消息加密类型
                encryptType = GetEncryptType();
                //如果已加密，解密消息
                string rawContent;
                if (encryptType == MessageEncryptTypeEnum.aes)
                    rawContent = DecryptMessage(content);
                else
                    rawContent = content;
                //解析消息
                if (!TryParse(rawContent, out message))
                {
                    Hoo.Common.WeChat.Message.Insert(new Message(Hoo.Common.WeChat.MessageType.Exception,
                        string.Format("解析消息失败。\r\n地址：{0}\r\n未解密的消息：{1}\r\n解密之后的消息：{2}",
                        request.RawUrl, content, rawContent)));
                }
                else
                {
                    Hoo.Common.WeChat.Message.Insert(new Message(Hoo.Common.WeChat.MessageType.Exception,
                        string.Format("解析消息成功。\r\n地址：{0}\r\n未解密的消息：{1}\r\n解密之后的消息：{2}",
                        request.RawUrl, content, message)));
                }
            }
            else
            {
                this.request = null;
                message = null;
            }
        }

        /// <summary>
        /// 得到消息加密类型
        /// </summary>
        /// <returns>返回消息加密类型</returns>
        private MessageEncryptTypeEnum GetEncryptType()
        {
            string encrypt_type = RequestEx.TryGetQueryString("encrypt_type", MessageEncryptTypeEnum.raw.ToString("g"), request);
            return (MessageEncryptTypeEnum)Enum.Parse(typeof(MessageEncryptTypeEnum), encrypt_type, true);
        }

        /// <summary>
        /// 解密消息
        /// </summary>
        /// <returns>返回解密之后的消息</returns>
        private string DecryptMessage(string content)
        {
            string msg = "";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(content);
            //XmlNode root = doc.FirstChild;
            //string userName = root["ToUserName"].InnerText;
            //AccountInfo account = AccountInfoCollection.GetAccountInfo(userName);
            //if (account == null)
            //    return msg;
            //Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(account.Token, account.EncodingAESKey, account.AppId);
            //string msg_signature = RequestEx.TryGetQueryString("msg_signature", "", request);
            //string timestamp = RequestEx.TryGetQueryString("timestamp", "", request);
            //string nonce = RequestEx.TryGetQueryString("nonce", "", request);
            //wxcpt.DecryptMsg(msg_signature, timestamp, nonce, content, ref msg);
            return msg;
        }

        /// <summary>
        /// 从xml字符串解析消息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>返回消息</returns>
        public static RequestBaseMessage Parse(string xml)
        {
            RequestBaseMessage msg = null;
            //将xml字符串解析成JObject对象
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            JObject jo = (JObject)JObject.Parse(json)["xml"];
            //解析消息基类
            msg = ParseBaseMessage(jo);
            //获取各分类的字段，并构造消息
            switch (msg.MsgType)
            {
                case RequestMessageTypeEnum.text:
                    msg = ParseTextMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.image:
                    msg = ParseImageMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.voice:
                    msg = ParseVoiceMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.video:
                    msg = ParseVideoMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.location:
                    msg = ParseLocationMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.link:
                    msg = ParseLinkMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.Event:
                    msg = ParseEventMessage(msg, jo);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现消息类型{0:g}解析。", msg.MsgType));
            }
            //返回
            return msg;
        }

        /// <summary>
        /// 尝试从xml字符串解析消息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="msg">如果解析成功，返回消息；否则，返回null。</param>
        /// <returns>返回解析是否成功</returns>
        private static bool TryParse(string xml, out RequestBaseMessage msg)
        {
            bool success = false;
            msg = null;
            try
            {
                msg = Parse(xml);
                success = true;
            }
            catch { }
            return success;
        }

        /// <summary>
        /// 判断JObject对象是否包含指定的属性
        /// </summary>
        /// <param name="jo">JObject对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回是否包含</returns>
        private static bool Contains(JObject jo, string propertyName)
        {
            JToken jt;
            return jo.TryGetValue(propertyName, out jt);
        }

        /// <summary>
        /// 解析消息基类
        /// </summary>
        /// <param name="jo">消息对象</param>
        /// <returns>返回消息基类</returns>
        private static RequestBaseMessage ParseBaseMessage(JObject jo)
        {
            string toUserName, fromUserName, strMsgType;
            DateTime createTime;
            RequestMessageTypeEnum msgType;
            toUserName = (string)jo["ToUserName"][CDATA_KEY];
            fromUserName = (string)jo["FromUserName"][CDATA_KEY];
            createTime = Utility.ToDateTime((int)jo["CreateTime"]);
            strMsgType = (string)jo["MsgType"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(toUserName))
                throw new ArgumentNullException("ToUserName", "ToUserName为空。");
            if (string.IsNullOrWhiteSpace(fromUserName))
                throw new ArgumentNullException("FromUserName", "FromUserName为空。");
            if (string.IsNullOrWhiteSpace(strMsgType))
                throw new ArgumentNullException("MsgType", "MsgType为空。");
            msgType = (RequestMessageTypeEnum)Enum.Parse(typeof(RequestMessageTypeEnum), strMsgType, true);
            return new RequestBaseMessage(toUserName, fromUserName, createTime, msgType);
        }

        /// <summary>
        /// 解析文本消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回文本消息</returns>
        private static RequestTextMessage ParseTextMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string content = (string)jo["Content"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("Content", "Content为空。");
            return new RequestTextMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, content, msgId);
        }

        /// <summary>
        /// 解析图片消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回图片消息</returns>
        private static RequestImageMessage ParseImageMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string picUrl = (string)jo["PicUrl"][CDATA_KEY];
            string mediaId = (string)jo["MediaId"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(picUrl))
                throw new ArgumentNullException("PicUrl", "PicUrl为空。");
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException("MediaId", "MediaId为空。");
            return new RequestImageMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, picUrl, mediaId, msgId);
        }

        /// <summary>
        /// 解析语音消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回语音消息</returns>
        private static RequestVoiceMessage ParseVoiceMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string mediaId = (string)jo["MediaId"][CDATA_KEY];
            string format = (string)jo["Format"][CDATA_KEY];
            string recognition = Contains(jo, "Recognition") ? (string)jo["Recognition"][CDATA_KEY] : null;
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException("MediaId", "MediaId为空。");
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException("Format", "Format为空。");
            return new RequestVoiceMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                mediaId, format, recognition, msgId);
        }

        /// <summary>
        /// 解析视频消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回视频消息</returns>
        private static RequestVideoMessage ParseVideoMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string mediaId = (string)jo["MediaId"][CDATA_KEY];
            string thumbMediaId = (string)jo["ThumbMediaId"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException("MediaId", "MediaId为空。");
            if (string.IsNullOrWhiteSpace(thumbMediaId))
                throw new ArgumentNullException("ThumbMediaId", "ThumbMediaId为空。");
            return new RequestVideoMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, mediaId, thumbMediaId, msgId);
        }

        /// <summary>
        /// 解析地理位置消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回地理位置消息</returns>
        private static RequestLocationMessage ParseLocationMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            double locationX = (double)jo["Location_X"];
            double locationY = (double)jo["Location_Y"];
            int scale = (int)jo["Scale"];
            string label = (string)jo["Label"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            return new RequestLocationMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, locationX, locationY, scale, label, msgId);
        }

        /// <summary>
        /// 解析链接消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回链接消息</returns>
        private static RequestLinkMessage ParseLinkMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string title = (string)jo["Title"][CDATA_KEY];
            string description = (string)jo["Description"][CDATA_KEY];
            string url = (string)jo["Url"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("Title", "Title为空。");
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("Url", "Url为空。");
            return new RequestLinkMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, title, description, url, msgId);
        }

        /// <summary>
        /// 解析事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回事件消息</returns>
        private static RequestBaseMessage ParseEventMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            RequestBaseMessage msg = null;
            string strEvent = (string)jo["Event"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(strEvent))
                throw new ArgumentNullException("Event", "Event为空。");
            RequestEventTypeEnum eventType = (RequestEventTypeEnum)Enum.Parse(typeof(RequestEventTypeEnum), strEvent, true);
            switch (eventType)
            {
                case RequestEventTypeEnum.subscribe:
                    msg = ParseSubscribeMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.unsubscribe:
                    msg = ParseUnsubscribeMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.SCAN:
                    msg = ParseScanMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.LOCATION:
                    msg = ParseReportLocationMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.CLICK:
                    msg = ParseClickMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.VIEW:
                    msg = ParseViewMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.scancode_push:
                case RequestEventTypeEnum.scancode_waitmsg:
                    msg = ParseScanCodeMessage(baseMessage, eventType, jo);
                    break;
                case RequestEventTypeEnum.pic_sysphoto:
                case RequestEventTypeEnum.pic_photo_or_album:
                case RequestEventTypeEnum.pic_weixin:
                    msg = ParsePicMessage(baseMessage, eventType, jo);
                    break;
                case RequestEventTypeEnum.location_select:
                    msg = ParseLocationSelectMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.MASSSENDJOBFINISH:
                    msg = ParseMassSendJobFinishMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.TEMPLATESENDJOBFINISH:
                    msg = ParseTemplateSendJobFinishMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.kf_create_session:
                    msg = ParseKfCreateAccountMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.kf_close_session:
                    msg = ParseKfCloseAccountMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.kf_switch_session:
                    msg = ParseKfSwitchAccountMessage(baseMessage, jo);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现消息类型{0:g}解析。", msg.MsgType));
            }
            return msg;
        }

        /// <summary>
        /// 解析订阅消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回订阅消息</returns>
        private static RequestSubscribeMessage ParseSubscribeMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string eventKey = Contains(jo, "EventKey") ? (string)jo["EventKey"][CDATA_KEY] : null;
            string ticket = Contains(jo, "Ticket") ? (string)jo["Ticket"][CDATA_KEY] : null;
            return new RequestSubscribeMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey, ticket);
        }

        /// <summary>
        /// 解析取消订阅消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回取消订阅消息</returns>
        private static RequestUnsubscribeMessage ParseUnsubscribeMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            return new RequestUnsubscribeMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime);
        }

        /// <summary>
        /// 解析扫描二维码消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回扫描二维码消息</returns>
        private static RequestScanMessage ParseScanMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string strEventKey = (string)jo["EventKey"][CDATA_KEY];
            string ticket = (string)jo["Ticket"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(strEventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            if (string.IsNullOrWhiteSpace(ticket))
                throw new ArgumentNullException("Ticket", "Ticket为空。");
            uint eventKey = uint.Parse(strEventKey);
            return new RequestScanMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey, ticket);
        }

        /// <summary>
        /// 解析上报地理位置消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回上报地理位置消息</returns>
        private static RequestReportLocationMessage ParseReportLocationMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            double latitude = (double)jo["Latitude"];
            double longitude = (double)jo["Longitude"];
            double precision = (double)jo["Precision"];
            return new RequestReportLocationMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                latitude, longitude, precision);
        }

        /// <summary>
        /// 解析点击菜单消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回点击菜单消息</returns>
        private static RequestClickMessage ParseClickMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string eventKey = (string)jo["EventKey"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(eventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            return new RequestClickMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey);
        }

        /// <summary>
        /// 解析点击菜单跳转链接消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回点击菜单跳转链接消息</returns>
        private static RequestViewMessage ParseViewMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string eventKey = (string)jo["EventKey"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(eventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            return new RequestViewMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey);
        }

        /// <summary>
        /// 解析扫码的推送事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回扫码的推送事件消息</returns>
        private static RequestScanCodeMessage ParseScanCodeMessage(RequestBaseMessage baseMessage, RequestEventTypeEnum eventType, JObject jo)
        {
            string eventKey = (string)jo["EventKey"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(eventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            JObject joScanCodeInfo = (JObject)jo["ScanCodeInfo"];
            string scanType = (string)joScanCodeInfo["ScanType"][CDATA_KEY];
            string scanResult = (string)joScanCodeInfo["ScanResult"][CDATA_KEY];
            return new RequestScanCodeMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventType, eventKey, scanType, scanResult);
        }

        /// <summary>
        /// 解析弹出发送图片的推送事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回弹出发送图片的推送事件消息</returns>
        private static RequestPicMessage ParsePicMessage(RequestBaseMessage baseMessage, RequestEventTypeEnum eventType, JObject jo)
        {
            string eventKey = (string)jo["EventKey"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(eventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            JObject joSendPicsInfo = (JObject)jo["SendPicsInfo"];
            int count = (int)joSendPicsInfo["Count"];
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count", "解析弹出发送图片的推送事件消息出错，图片数量必须大于0。");
            else
            {

                JToken jtPicList = joSendPicsInfo["PicList"];
                if (jtPicList.Type == JTokenType.Array)
                {
                    string[] pics = new string[count];
                    JArray jaPicList = (JArray)jtPicList;
                    int idx = 0;
                    foreach (JObject joItem in jaPicList)
                    {
                        string pic = (string)joItem["item"]["PicMd5Sum"][CDATA_KEY];
                        pics[idx] = pic;
                        idx++;
                    }
                    return new RequestPicMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                    eventType, eventKey, pics);
                }
                else if (jtPicList.Type == JTokenType.Object)
                {
                    JObject joPicList = (JObject)jtPicList;
                    string pic = (string)joPicList["item"]["PicMd5Sum"][CDATA_KEY];
                    return new RequestPicMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                    eventType, eventKey, pic);
                }
                else
                    throw new ArgumentOutOfRangeException("PicList", "解析弹出发送图片的推送事件消息出错，图片列表不正确。");
            }
        }

        /// <summary>
        /// 解析弹出地理位置选择的推送事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回弹出地理位置选择的推送事件消息</returns>
        private static RequestLocationSelectMessage ParseLocationSelectMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string eventKey = (string)jo["EventKey"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(eventKey))
                throw new ArgumentNullException("EventKey", "EventKey为空。");
            JObject joSendLocationInfo = (JObject)jo["SendLocationInfo"];
            double locationX = (double)joSendLocationInfo["Location_X"][CDATA_KEY];
            double locationY = (double)joSendLocationInfo["Location_Y"][CDATA_KEY];
            int scale = (int)joSendLocationInfo["Scale"][CDATA_KEY];
            string label = (string)joSendLocationInfo["Label"][CDATA_KEY];
            string poiname = Contains(joSendLocationInfo, "Poiname") ? (string)joSendLocationInfo["Poiname"][CDATA_KEY] : null;
            return new RequestLocationSelectMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey, locationX, locationY, scale, label, poiname);
        }

        /// <summary>
        /// 解析推送群发消息结果的消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回推送群发消息结果的消息</returns>
        private static RequestMassSendJobFinishMessage ParseMassSendJobFinishMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            long msgId = (long)jo["MsgID"];
            string status = (string)jo["Status"][CDATA_KEY];
            int totalCount = (int)jo["TotalCount"];
            int filterCount = (int)jo["FilterCount"];
            int sentCount = (int)jo["SentCount"];
            int errorCount = (int)jo["ErrorCount"];
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentNullException("Status", "Status为空。");
            return new RequestMassSendJobFinishMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                msgId, status, totalCount, filterCount, sentCount, errorCount);
        }

        /// <summary>
        /// 解析推送发送模板消息结果的消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回推送发送模板消息结果的消息</returns>
        private static RequestTemplateSendJobFinishMessage ParseTemplateSendJobFinishMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            long msgId = (long)jo["MsgID"];
            string status = (string)jo["Status"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentNullException("Status", "Status为空。");
            return new RequestTemplateSendJobFinishMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                msgId, status);
        }

        /// <summary>
        /// 解析客服接入会话事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回客服接入会话事件消息</returns>
        private static RequestKfCreateSessionMessage ParseKfCreateAccountMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string kfAccount = (string)jo["KfAccount"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentNullException("KfAccount", "KfAccount为空。");
            return new RequestKfCreateSessionMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                kfAccount);
        }

        /// <summary>
        /// 解析客服关闭会话事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回客服关闭会话事件消息</returns>
        private static RequestKfCloseSessionMessage ParseKfCloseAccountMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string kfAccount = (string)jo["KfAccount"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentNullException("KfAccount", "KfAccount为空。");
            return new RequestKfCloseSessionMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                kfAccount);
        }

        /// <summary>
        /// 解析客服接入会话事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回客服接入会话事件消息</returns>
        private static RequestKfSwitchSessionMessage ParseKfSwitchAccountMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string fromKfAccount = (string)jo["FromKfAccount"][CDATA_KEY];
            string toKfAccount = (string)jo["ToKfAccount"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(fromKfAccount))
                throw new ArgumentNullException("FromKfAccount", "FromKfAccount为空。");
            if (string.IsNullOrWhiteSpace(toKfAccount))
                throw new ArgumentNullException("ToKfAccount", "ToKfAccount为空。");
            return new RequestKfSwitchSessionMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                fromKfAccount, toKfAccount);
        }
    }
}