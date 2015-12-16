using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 扫码的事件推送消息
    /// </summary>
    public class RequestScanCodeMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取事件KEY值
        /// </summary>
        public string EventKey { get; private set; }
        /// <summary>
        /// 获取扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventType"></param>
        /// <param name="eventKey"></param>
        /// <param name="scanCodeInfo"></param>
        public RequestScanCodeMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType, string eventKey, ScanCodeInfo scanCodeInfo)
            : base(toUserName, fromUserName, createTime, eventType)
        {
            if (eventType != RequestEventTypeEnum.scancode_push &&
                eventType != RequestEventTypeEnum.scancode_waitmsg)
                throw new ArgumentOutOfRangeException("EventType", "事件类型错误。");
            ScanCodeInfo = scanCodeInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="eventType"></param>
        /// <param name="eventKey"></param>
        /// <param name="scanType"></param>
        /// <param name="scanResult"></param>
        public RequestScanCodeMessage(string toUserName, string fromUserName, DateTime createTime,
            RequestEventTypeEnum eventType, string eventKey, string scanType, string scanResult)
            : this(toUserName, fromUserName, createTime, eventType, eventKey, new ScanCodeInfo(scanType, scanResult))
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n事件KEY值：{1}\r\n扫描信息：{2}",
                base.ToString(), EventKey, ScanCodeInfo);
        }
    }
}
