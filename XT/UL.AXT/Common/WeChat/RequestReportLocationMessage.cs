using System;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 上报地理位置消息
    /// </summary>
    public class RequestReportLocationMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取纬度
        /// </summary>
        public double Latitude { get; private set; }
        /// <summary>
        /// 获取经度
        /// </summary>
        public double Longitude { get; private set; }
        /// <summary>
        /// 获取精度
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="precision"></param>
        public RequestReportLocationMessage(string toUserName, string fromUserName, DateTime createTime,
            double latitude, double longitude, double precision)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.LOCATION)
        {
            Latitude = latitude;
            Longitude = longitude;
            Precision = precision;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n纬度：{1}\r\n经度：{2}\r\n精度：{3}",
                base.ToString(), Latitude, Longitude, Precision);
        }
    }
}