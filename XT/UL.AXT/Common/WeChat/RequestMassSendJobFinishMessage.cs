using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 推送群发消息结果的消息
    /// </summary>
    public class RequestMassSendJobFinishMessage : RequestEventMessage
    {
        /// <summary>
        /// 发送成功
        /// </summary>
        private const string sendSuccess = "send success";
        /// <summary>
        /// 发送失败
        /// </summary>
        private const string sendFailed = "send fail";
        /// <summary>
        /// 审核失败字典
        /// </summary>
        private static readonly Dictionary<string, string> errorDict = new Dictionary<string, string>()
        {
            {"err(10001)","涉嫌广告"},
            {"err(20001)","涉嫌政治"},
            {"err(20004)","涉嫌社会"},
            {"err(20002)","涉嫌色情"},
            {"err(20006)","涉嫌违法犯罪"},
            {"err(20008)","涉嫌欺诈"},
            {"err(20013)","涉嫌版权"},
            {"err(22000)","涉嫌互推（互相宣传）"},
            {"err(21000)","涉嫌其他"}
        };
        /// <summary>
        /// 获取消息id
        /// </summary>
        public long MsgID { get; private set; }
        /// <summary>
        /// 获取群发消息的结果
        /// </summary>
        public string Status { get; private set; }
        /// <summary>
        /// 获取用户总数
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// 获取过滤后待发送的用户数
        /// </summary>
        public int FilterCount { get; private set; }
        /// <summary>
        /// 获取发送成功的用户数
        /// </summary>
        public int SentCount { get; private set; }
        /// <summary>
        /// 获取发送失败的用户数
        /// </summary>
        public int ErrorCount { get; private set; }

        /// <summary>
        /// 获取消息是否群发成功
        /// </summary>
        public bool SendSuccess
        {
            get
            {
                return Status == sendSuccess;
            }
        }
        /// <summary>
        /// 获取发送失败的原因
        /// </summary>
        public string ErrorReason
        {
            get
            {
                string reason = string.Empty;
                if (Status == sendSuccess)
                    reason = "发送成功";
                else if (Status == sendFailed)
                    reason = "发送失败";
                else if (errorDict.ContainsKey(Status))
                    reason = errorDict[Status];
                return reason;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="msgId"></param>
        /// <param name="status"></param>
        /// <param name="totalCount"></param>
        /// <param name="filterCount"></param>
        /// <param name="sentCount"></param>
        /// <param name="errorCount"></param>
        public RequestMassSendJobFinishMessage(string toUserName, string fromUserName, DateTime createTime,
            long msgId, string status, int totalCount, int filterCount, int sentCount, int errorCount)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.MASSSENDJOBFINISH)
        {
            MsgID = msgId;
            Status = status;
            TotalCount = totalCount;
            FilterCount = filterCount;
            SentCount = sentCount;
            ErrorCount = errorCount;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n消息id：{1}\r\n群发结果：{2}\r\n用户总数：{3}\r\n" +
            "过滤后待发送的用户数：{4}\r\n发送成功的用户数：{5}\r\n发送失败的用户数：{6}\r\n是否群发成功：{7}\r\n发送失败的原因：{8}",
                base.ToString(), MsgID, Status, TotalCount,
                FilterCount, SentCount, ErrorCount, SendSuccess, ErrorReason);
        }
    }
}