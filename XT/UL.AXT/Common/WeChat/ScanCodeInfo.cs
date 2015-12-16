using System;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 扫描信息
    /// </summary>
    public class ScanCodeInfo
    {
        /// <summary>
        /// 扫描类型，一般是qrcode
        /// </summary>
        public string ScanType { get; set; }
        /// <summary>
        /// 扫描结果，即二维码对应的字符串信息
        /// </summary>
        public string ScanResult { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">扫描类型，一般是qrcode</param>
        /// <param name="result">扫描结果，即二维码对应的字符串信息</param>
        public ScanCodeInfo(string type,string result)
        {
            ScanType = type;
            ScanResult = result;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("扫描类型：{0}\r\n扫描结果：{1}\r\n", ScanType, ScanResult);
        }
    }
}
