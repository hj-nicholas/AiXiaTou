using System;
using System.Xml;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 请求消息基类
    /// </summary>
    public class RequestBaseMessage
    {
        /// <summary>
        /// 获取开发者微信号
        /// </summary>
        public String ToUserName { get; protected set; }
        /// <summary>
        /// 获取发送方帐号（一个OpenID）
        /// </summary>
        public String FromUserName { get; protected set; }
        /// <summary>
        /// 获取消息创建时间  
        /// </summary>
        public DateTime CreateTime { get; protected set; }
        /// <summary>
        /// 获取消息类型
        /// </summary>
        public RequestMessageTypeEnum MsgType { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="fromUserName">发送方账号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="msgType">消息类型</param>
        public RequestBaseMessage(string toUserName,string fromUserName,DateTime createTime, RequestMessageTypeEnum msgType)
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
            return string.Format("开发者微信号：{0}\r\n发送方账号：{1}\r\n消息创建时间：{2}\r\n消息类型：{3:g}",
                ToUserName, FromUserName, CreateTime, MsgType);
        }
    }
}