using System;
using System.Text;
using System.Collections.Generic;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 发送的图片信息
    /// </summary>
    public class SendPicsInfo
    {
        /// <summary>
        /// 图片列表
        /// </summary>
        private string[] pics;

        /// <summary>
        /// 获取发送的图片数量
        /// </summary>
        public int Count
        {
            get
            {
                return pics.Length;
            }
        }

        /// <summary>
        /// 获取图片列表
        /// </summary>
        public string[] PicList
        {
            get
            {
                return pics;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pics"></param>
        public SendPicsInfo(IEnumerable<string> pics)
        {
            if (pics == null)
                throw new ArgumentNullException("pics", "图片列表不能为空。");
            this.pics = new List<string>(pics).ToArray();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pic"></param>
        public SendPicsInfo(string pic)
        {
            this.pics = new string[] { pic };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string newLine = Environment.NewLine;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("图片数量：{0}{1}", Count, newLine);
            if(pics.Length>0)
            {
                sb.Append("图片列表：");
                foreach (string pic in pics)
                    sb.AppendFormat("{0},", pic);
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}
