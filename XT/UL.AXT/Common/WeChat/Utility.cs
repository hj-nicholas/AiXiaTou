using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// WeixinUtility 的摘要说明
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 微信中进行哈希或加解密的编码
        /// </summary>
        private static readonly Encoding hashEncoding = Encoding.ASCII;

        /// <summary>
        /// 微信的基准时间
        /// </summary>
        private static readonly DateTime baseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="token">令牌</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns>返回验证是否通过</returns>
        public static bool CheckSignature(string signature, string token, string timestamp, string nonce)
        {
            string[] infos = new string[] { token, timestamp, nonce };
            Array.Sort<string>(infos);
            string info = string.Format("{0}{1}{2}", infos[0], infos[1], infos[2]);
            return string.Compare(signature, GetSha1Hash(info, hashEncoding), true) == 0;
        }

        /// <summary>
        /// 得到输入字符串的SHA1哈希字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding">编码</param>
        /// <returns>返回SHA1哈希字符串</returns>
        public static string GetSha1Hash(string input, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(input) && encoding != null)
            {
                byte[] bytes = encoding.GetBytes(input);
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] result = sha1.ComputeHash(bytes);
                foreach (byte b in result)
                    sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将微信服务器返回的表示日期时间的秒数，转换成日期时间。
        /// 秒数的计时起点为1970年1月1日0点。
        /// 注：微信采用UTC时间，需要转换成本地时间。
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns>返回日期时间</returns>
        public static DateTime ToDateTime(int seconds)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(baseTime.AddSeconds(seconds), TimeZoneInfo.Local);
        }

        /// <summary>
        /// 将微信服务器返回的表示日期时间的秒数，转换成日期时间。
        /// 秒数的计时起点为1970年1月1日0点。
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns>返回日期时间</returns>
        public static DateTime ToDateTime(string seconds)
        {
            int s;
            if (int.TryParse(seconds, out s))
                return ToDateTime(s);
            else
                return baseTime;
        }

        /// <summary>
        /// 返回微信时间（距1970年1月1日0点的秒数）
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>返回微信时间</returns>
        public static int ToWeixinTime(DateTime dt)
        {
            return (int)(dt - baseTime).TotalSeconds;
        }

        /// <summary>
        /// 获取请求的内容
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>返回内容</returns>
        public static string GetRequestContent(HttpRequest request)
        {
            string content = string.Empty;
            byte[] bytes = request.BinaryRead(request.ContentLength);
            if (bytes != null && bytes.Length > 0)
                content = request.ContentEncoding.GetString(bytes);
            return content;
        }

        /// <summary>
        /// 将颜色额转换成由16进制RGB分量表示的字符串。
        /// 例如：红色-#FF0000
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>返回颜色字符串</returns>
        public static string GetColorString(Color color)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
        }

        ///// <summary>
        ///// 从json字符串解析对象
        ///// </summary>
        ///// <typeparam name="T">实现了IParsable接口的类</typeparam>
        ///// <param name="json">json字符串</param>
        ///// <returns>返回对象</returns>
        //public static T Parse<T>(string json)
        //    where T : IParsable, new()
        //{
        //    return Parse<T>(JObject.Parse(json));
        //}

        ///// <summary>
        ///// 从JObject对象解析对象
        ///// </summary>
        ///// <typeparam name="T">实现了IParsable接口的类</typeparam>
        ///// <param name="jo">JObject对象</param>
        ///// <returns>返回对象</returns>
        //public static T Parse<T>(JObject jo)
        //    where T : IParsable, new()
        //{
        //    T t = new T();
        //    t.Parse(jo);
        //    return t;
        //}
    }
}