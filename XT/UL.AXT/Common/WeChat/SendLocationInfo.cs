using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 发送的位置信息
    /// </summary>
    public class SendLocationInfo
    {
        /// <summary>
        /// X坐标信息
        /// </summary>
        public double LocationX { get; set; }
        /// <summary>
        /// Y坐标信息
        /// </summary>
        public double LocationY { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 地理位置的字符串信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 朋友圈POI的名字
        /// </summary>
        public string Poiname { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="locationX">X坐标信息</param>
        /// <param name="locationY">Y坐标信息</param>
        /// <param name="scale">精度</param>
        /// <param name="label">地理位置的字符串信息</param>
        /// <param name="poiname">朋友圈POI的名字</param>
        public SendLocationInfo(double locationX, double locationY, int scale, string label, string poiname = null)
        {
            LocationX = locationX;
            LocationY = locationY;
            Scale = scale;
            Label = label;
            Poiname = poiname;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X坐标：{0}\r\nY坐标：{1}\r\n精度：{2}\r\n地理位置：{3}\r\n朋友圈POI的名字：{4}",
                LocationX, LocationY, Scale, Label, Poiname ?? string.Empty);
        }
    }
}