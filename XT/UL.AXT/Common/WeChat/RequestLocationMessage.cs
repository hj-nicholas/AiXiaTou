using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class RequestLocationMessage : RequestBaseMessage
    {
        /// <summary>
        /// 获取纬度
        /// </summary>
        public double LocationX { get; private set; }
        /// <summary>
        /// 获取经度
        /// </summary>
        public double LocationY { get; private set; }
        /// <summary>
        /// 获取地图缩放大小
        /// </summary>
        public int Scale { get; private set; }
        /// <summary>
        /// 获取地理位置信息
        /// </summary>
        public string Label { get; private set; }
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
        /// <param name="locationX"></param>
        /// <param name="locationY"></param>
        /// <param name="scale"></param>
        /// <param name="label"></param>
        /// <param name="msgId"></param>
        public RequestLocationMessage(string toUserName, string fromUserName, DateTime createTime,
            double locationX, double locationY, int scale, string label, long msgId)
            : base(toUserName, fromUserName, createTime, RequestMessageTypeEnum.location)
        {
            LocationX = locationX;
            LocationY = locationY;
            Scale = scale;
            Label = label;
            MsgId = msgId;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n纬度：{1}\r\n经度：{2}\r\n地图缩放大小：{3}\r\n地理位置信息：{4}\r\n消息ID：{5}",
                base.ToString(), LocationX, LocationY, Scale, Label, MsgId);
        }
    }
}