using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/*todo:该文件仅用于调试，正式部署请删除。*/
namespace Hoo.Common.WeChat
{
    /// <summary>
    /// 消息类型
    /// </summary>
    internal enum MessageType
    {
        /// <summary>
        /// 请求
        /// </summary>
        Request,
        /// <summary>
        /// 响应
        /// </summary>
        Response,
        /// <summary>
        /// 异常
        /// </summary>
        Exception
    }

    public enum MessageEncryptTypeEnum
    {
        /// <summary>
        /// 明文：请求与回复的消息体均为明文
        /// </summary>
        raw,
        /// <summary>
        /// aes密文：请求与回复的消息体均为aes加密之后的密文
        /// </summary>
        aes
    }

    /// <summary>
    /// Message：消息
    /// </summary>
    internal class Message
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        public Message(Guid id, MessageType type, string content, DateTime time)
        {
            Id = id;
            Type = type;
            Content = content;
            Time = time;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public Message(MessageType type, string content)
            : this(Guid.NewGuid(), type, content, DateTime.Now)
        {
        }

        /// <summary>
        /// 返回参数列表
        /// </summary>
        /// <returns></returns>
        private List<SqlParameter> ToParameters()
        {
            List<SqlParameter> parameters = new List<SqlParameter>(4);
            SqlParameter p = new SqlParameter("@Id", SqlDbType.UniqueIdentifier, 16);
            p.Value = Id;
            parameters.Add(p);
            p = new SqlParameter("@Type", SqlDbType.Int, 4);
            p.Value = (int)Type;
            parameters.Add(p);
            p = new SqlParameter("@Content", SqlDbType.NVarChar);
            p.Value = Content;
            parameters.Add(p);
            p = new SqlParameter("@Time", SqlDbType.DateTime, 8);
            p.Value = Time;
            parameters.Add(p);
            return parameters;
        }

        /// <summary>
        /// 插入新消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>返回插入是否成功</returns>
        public static bool Insert(Message message)
        {
            //string commandText = "INSERT INTO Message (Id,Type,Content,Time) VALUES (@Id,@Type,@Content,@Time)";
            //string result;
            //List<SqlParameter> parameters = message.ToParameters();
            //return DataAccess.ExecuteCommand(commandText, parameters, out result) > 0;
            return true;
        }
    }
}