using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 推送发送模板消息结果的消息
    /// </summary>
    public class RequestTemplateSendJobFinishMessage : RequestEventMessage
    {
        /// <summary>
        /// 发送成功
        /// </summary>
        private const string sendSuccess = "success";
        /// <summary>
        /// 发送失败：用户拒收
        /// </summary>
        private const string sendFailedUserBlock = "failed:user block";
        /// <summary>
        /// 发送失败：系统失败
        /// </summary>
        private const string sendFailedSystemFailed = "failed: system failed";

        /// <summary>
        /// 获取消息id
        /// </summary>
        public long MsgID { get; private set; }
        /// <summary>
        /// 获取群发消息的结果
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// 获取消息是否群发成功
        /// </summary>
        public TemplateMessageSendStatusEnum SendStatus
        {
            get
            {
                TemplateMessageSendStatusEnum status;
                if (Status == sendFailedUserBlock)
                    status = TemplateMessageSendStatusEnum.UserBlock;
                else if (Status == sendFailedSystemFailed)
                    status = TemplateMessageSendStatusEnum.SystemFailed;
                else
                    status = TemplateMessageSendStatusEnum.Success;
                return status;
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
        public RequestTemplateSendJobFinishMessage(string toUserName, string fromUserName, DateTime createTime,
            long msgId, string status)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.TEMPLATESENDJOBFINISH)
        {
            MsgID = msgId;
            Status = status;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n消息id：{1}\r\n群发结果：{2}",
                base.ToString(), MsgID, Status);
        }
    }

    public enum TemplateMessageSendStatusEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败：用户拒收
        /// </summary>
        UserBlock,
        /// <summary>
        /// 失败：系统失败
        /// </summary>
        SystemFailed
    }
}