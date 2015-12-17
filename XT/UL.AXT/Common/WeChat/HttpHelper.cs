using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Hoo.Common.WeChat
{
    /// <summary>
    /// HttpHelper：http请求与响应辅助类
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 向微信服务器发送请求时的编码
        /// </summary>
        public static readonly Encoding RequestEncoding = Encoding.UTF8;
        /// <summary>
        /// 微信服务器响应的编码
        /// </summary>
        public static readonly Encoding ResponseEncoding = Encoding.UTF8;

        /// <summary>
        /// 向微信服务器提交数据，并获取微信服务器响应的数据
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseData">返回响应数据</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, out byte[] responseData,
            string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
        {
            bool success = false;
            responseData = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream ms = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = httpMethod;
                if (data != null && data.Length > 0)
                {
                    request.ContentLength = data.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                //由于微信服务器的响应有时没有正确设置ContentLength，这里不检查ContentLength
                //if (response.ContentLength > 0)
                {
                    ms = new MemoryStream();
                    responseStream = response.GetResponseStream();
                    int bufferLength = 2048;
                    byte[] buffer = new byte[bufferLength];
                    int size = responseStream.Read(buffer, 0, bufferLength);
                    while (size > 0)
                    {
                        ms.Write(buffer, 0, size);
                        size = responseStream.Read(buffer, 0, bufferLength);
                    }
                    responseData = ms.ToArray();
                }
                success = true;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
                if (responseStream != null)
                    responseStream.Close();
                if (ms != null)
                    ms.Close();
                if (response != null)
                    response.Close();
            }
            return success;
        }

        /// <summary>
        /// 向微信服务器提交数据，并获取微信服务器响应的数据
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseData">返回响应数据</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, out byte[] responseData,
            string httpMethod = WebRequestMethods.Http.Get, string data = null)
        {
            byte[] bytes = string.IsNullOrEmpty(data) ? null : RequestEncoding.GetBytes(data);
            return Request(url, out responseData, httpMethod, (byte[])bytes);
        }

        /// <summary>
        /// 向微信服务器提交数据，并获取微信服务器响应的内容
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseContent">返回响应内容</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, out string responseContent,
            string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
        {
            byte[] responseData;
            responseContent = string.Empty;
            bool success = Request(url, out responseData, httpMethod, data);
            if (success && responseData != null && responseData.Length > 0)
                responseContent = ResponseEncoding.GetString(responseData);
            return success;
        }

        /// <summary>
        /// 向微信服务器提交数据，并获取微信服务器响应的内容
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseContent">返回响应内容</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, out string responseContent,
            string httpMethod = WebRequestMethods.Http.Get, string data = null)
        {
            byte[] bytes = string.IsNullOrEmpty(data) ? null : RequestEncoding.GetBytes(data);
            return Request(url, out responseContent, httpMethod, (byte[])bytes);
        }

        /// <summary>
        /// 向微信服务器提交数据
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
        {
            bool success = false;
            Stream requestStream = null;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = httpMethod;
                if (data != null && data.Length > 0)
                {
                    request.ContentLength = data.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                success = true;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
                if (response != null)
                    response.Close();
            }
            return success;
        }

        /// <summary>
        /// 组合url，发送数据，然后返回响应字符串
        /// </summary>
        /// <param name="urlFormat">url格式字符串，第一个参数为userName获取到的许可令牌，然后依次为parameters中的参数</param>
        /// <param name="userName">公众号</param>
        /// <param name="urlParameters">参数</param>
        /// <param name="httpMethod">执行请求的http方法</param>
        /// <param name="data">请求的内容</param>
        /// <returns>返回响应内容；如果请求失败，或者发生错误，返回空字符串</returns>
        public static string RequestResponseContent(string urlFormat, string userName, IEnumerable<object> urlParameters = null, string httpMethod = WebRequestMethods.Http.Get, string data = null)
        {
            string responseContent = string.Empty;
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
                return responseContent;
            string url;
            if (urlParameters == null)
                url = string.Format(urlFormat, token.access_token);
            else
            {
                List<object> paramList = new List<object>(urlParameters);
                paramList.Insert(0, token.access_token);
                url = string.Format(urlFormat, paramList.ToArray());
            }
            HttpHelper.Request(url, out responseContent, httpMethod, (string)data);
            return responseContent;
        }

        /// <summary>
        /// 组合url，发送数据，然后返回响应的错误消息。
        /// 注：错误消息不一定代表失败或者错误。
        /// </summary>
        /// <param name="urlFormat">url格式字符串，第一个参数为userName获取到的许可令牌，然后依次为parameters中的参数</param>
        /// <param name="userName">公众号</param>
        /// <param name="urlParameters">参数</param>
        /// <param name="httpMethod">执行请求的http方法</param>
        /// <param name="data">请求的内容</param>
        /// <returns>返回响应的错误消息</returns>
        public static ErrorMessage RequestErrorMessage(string urlFormat, string userName, IEnumerable<object> urlParameters = null, string httpMethod = WebRequestMethods.Http.Get, string data = null)
        {
            string responseContent = RequestResponseContent(urlFormat, userName, urlParameters, httpMethod, data);
            if (string.IsNullOrWhiteSpace(responseContent))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                return ErrorMessage.Parse(responseContent);
            else
                return new ErrorMessage(ErrorMessage.ExceptionCode, "解析响应失败。");
        }

        /// <summary>
        /// 组合url，发送数据，然后返回结果。
        /// 注：结果为需要解析的类。
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="urlFormat">url格式字符串，第一个参数为userName获取到的许可令牌，然后依次为parameters中的参数</param>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回请求是否成功</param>
        /// <param name="urlParameters">参数</param>
        /// <param name="httpMethod">执行请求的http方法</param>
        /// <param name="data">请求的内容</param>
        /// <returns>返回结果；如果请求失败，或者发生错误，返回null。</returns>
        public static T RequestParsableResult<T>(string urlFormat, string userName, out ErrorMessage errorMessage, IEnumerable<object> urlParameters = null, string httpMethod = WebRequestMethods.Http.Get, string data = null)
            where T : IParsable, new()
        {
            T result = default(T);
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            string responseContent = RequestResponseContent(urlFormat, userName, urlParameters, httpMethod, data);
            if (string.IsNullOrWhiteSpace(responseContent))
                return result;
            if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                try
                {
                    result = Utility.Parse<T>(responseContent);
                    if (result != null)
                        errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                    else
                        errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "解析失败。");
                }
                catch
                {
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "解析失败。");
                }
            }
            return result;
        }

        /// <summary>
        /// 组合url，发送数据，然后返回结果。
        /// 注：结果为已知的简单值类型。
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="urlFormat">url格式字符串，第一个参数为userName获取到的许可令牌，然后依次为parameters中的参数</param>
        /// <param name="userName">公众号</param>
        /// <param name="propertyNameInJson">返回结果在json中的键名</param>
        /// <param name="errorMessage">返回请求是否成功</param>
        /// <param name="urlParameters">参数</param>
        /// <param name="httpMethod">执行请求的http方法</param>
        /// <param name="data">请求的内容</param>
        /// <returns>返回结果；如果请求失败，或者发生错误，返回default(T)。</returns>
        public static T RequestValueTypeResult<T>(string urlFormat, string userName, string propertyNameInJson, out ErrorMessage errorMessage, IEnumerable<object> urlParameters = null, string httpMethod = WebRequestMethods.Http.Get, string data = null)
            where T : struct
        {
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            string responseContent = RequestResponseContent(urlFormat, userName, urlParameters, httpMethod, data);
            return ConvertValueTypeResult<T>(responseContent, propertyNameInJson, out errorMessage);
        }

        /// <summary>
        /// 获取值类型的结果
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="responseContent">响应内容</param>
        /// <param name="propertyNameInJson">返回结果在json中的键名</param>
        /// <param name="errorMessage">返回请求是否成功</param>
        /// <returns>返回结果；如果请求失败，或者发生错误，返回default(T)。</returns>
        private static T ConvertValueTypeResult<T>(string responseContent, string propertyNameInJson, out ErrorMessage errorMessage)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
                return default(T);
            }
            if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
            JObject jo = JObject.Parse(responseContent);
            JToken jt;
            if (jo.TryGetValue(propertyNameInJson, out jt))
                return ConvertValueTypeResult<T>((string)jt);
            else
                return default(T);
        }

        /// <summary>
        /// 获取值类型的结果
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="responseContent">响应内容</param>
        /// <param name="propertyNameInJson">返回结果在json中的键名</param>
        /// <param name="errorMessage">返回请求是否成功</param>
        /// <returns>返回结果；如果请求失败，或者发生错误，返回default(T)。</returns>
        private static T ConvertValueTypeResult<T>(string value)
            where T : struct
        {
            Type type = typeof(T);
            if (type.IsEnum)
                return (T)Enum.Parse(type, value);
            else if (type == typeof(sbyte))
                return (T)(object)Convert.ToSByte(value);
            else if (type == typeof(byte))
                return (T)(object)Convert.ToByte(value);
            else if (type == typeof(char))
                return (T)(object)Convert.ToChar(value);
            else if (type == typeof(short))
                return (T)(object)Convert.ToInt16(value);
            else if (type == typeof(ushort))
                return (T)(object)Convert.ToUInt16(value);
            else if (type == typeof(int))
                return (T)(object)Convert.ToInt32(value);
            else if (type == typeof(uint))
                return (T)(object)Convert.ToUInt32(value);
            else if (type == typeof(long))
                return (T)(object)Convert.ToInt64(value);
            else if (type == typeof(ulong))
                return (T)(object)Convert.ToUInt64(value);
            else if (type == typeof(float))
                return (T)(object)Convert.ToSingle(value);
            else if (type == typeof(double))
                return (T)(object)Convert.ToDouble(value);
            else if (type == typeof(decimal))
                return (T)(object)Convert.ToDecimal(value);
            else if (type == typeof(bool))
                return (T)(object)Convert.ToBoolean(value);
            else
                throw new ArgumentException("不支持的值类型。");
        }

        /// <summary>
        /// 向微信服务器提交数据
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        public static bool Request(string url, string httpMethod = WebRequestMethods.Http.Get, string data = null)
        {
            byte[] bytes = string.IsNullOrEmpty(data) ? null : RequestEncoding.GetBytes(data);
            return Request(url, httpMethod, (byte[])bytes);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="filename">文件名（不包含路径）</param>
        /// <param name="fileData">文件数据</param>
        /// <param name="formData">表单数据</param>
        /// <returns>返回服务器的响应字符串</returns>
        public static string Upload(string url, string filename, byte[] fileData, NameValueCollection formData = null)
        {
            string responseContent = string.Empty;
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(filename) || fileData == null || fileData.Length == 0)
                return responseContent;
            // 边界符
            string boundary = "AaB03xAaB03x";
            // 开始边界符
            byte[] beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 结束符
            byte[] endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
            //换行
            byte[] newLine = Encoding.ASCII.GetBytes("\r\n");
            MemoryStream ms = null;
            Stream stream = null;
            HttpWebResponse response = null;
            StreamReader sr = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                // 写入文件
                string header = string.Format("Content-Disposition: form-data; filename=\"{0}\"\r\n" +
                     "Content-Type: application/octet-stream\r\n\r\n",
                     filename);
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                ms = new MemoryStream();
                ms.Write(beginBoundary, 0, beginBoundary.Length);
                ms.Write(headerbytes, 0, headerbytes.Length);
                ms.Write(fileData, 0, fileData.Length);
                // 写入表单数据
                if (formData != null && formData.Count > 0)
                {
                    var formItem = "\r\n--" + boundary +
                                           "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                           "\r\n\r\n{1}\r\n";
                    foreach (string key in formData.Keys)
                    {
                        string value = formData[key];
                        byte[] bytes = Encoding.UTF8.GetBytes(string.Format(formItem, key, value));
                        ms.Write(bytes, 0, bytes.Length);
                    }
                }
                //写入结束边界符
                ms.Write(newLine, 0, newLine.Length);
                ms.Write(endBoundary, 0, endBoundary.Length);
                request.ContentLength = ms.Length;
                stream = request.GetRequestStream();
                stream.Write(ms.ToArray(), 0, (int)ms.Length);
                //获取响应
                response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), HttpHelper.ResponseEncoding);
                responseContent = sr.ReadToEnd();
            }
            finally
            {
                if (ms != null)
                    ms.Close();
                if (stream != null)
                    stream.Close();
                if (sr != null)
                    sr.Close();
                if (response != null)
                    response.Close();
            }
            return responseContent;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="pathname">包含路径的文件名</param>
        /// <param name="formData">表单数据</param>
        /// <returns>返回服务器的响应字符串</returns>
        public static string Upload(string url, string pathname, NameValueCollection formData = null)
        {
            string filename = Path.GetFileName(pathname);
            byte[] data = null;
            FileStream fs = null;
            MemoryStream ms = null;
            try
            {
                fs = new FileStream(pathname, FileMode.Open, FileAccess.Read);
                ms = new MemoryStream();
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int size = fs.Read(buffer, 0, bufferLength);
                while (size > 0)
                {
                    ms.Write(buffer, 0, size);
                    size = fs.Read(buffer, 0, bufferLength);
                }
                data = ms.ToArray();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (ms != null)
                    ms.Close();
            }
            return Upload(url, filename, data, formData);
        }
    }
}