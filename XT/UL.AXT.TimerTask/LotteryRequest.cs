using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Xfrog.Net;

namespace UL.AXT.TimerTask
{
  public  class LotteryRequest
    {
       
      public LotPerModel GetLottery()
      {
            string appkey = "488b6613d9dfc91e507e99eaba27bfb6e1dd5e65"; //配置您申请的appkey


            //1.高频彩票列表查询
            //string url1 = "http://op.juhe.cn/lottery/list";
          string url1 = "http://api.caipiaokong.com/lottery/";

            var parameters1 = new Dictionary<string, string>();

            parameters1.Add("token", appkey);//你申请的key
            parameters1.Add("format", "xml"); //返回数据的格式,xml或json，默认json
            parameters1.Add("name", "cqssc");//彩票类型
            parameters1.Add("uid", "207601");//用户Uid
            parameters1.Add("num", "1");//获取数量
            string result = sendPost(url1, parameters1, "get");
            //string result= "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><root><item id = \"20160123049\" dateline = \"2016-01-23 14:11:16\" > 9,3,4,2,6 </item></root> ";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            string lotCot = doc.ChildNodes[1].ChildNodes[0].InnerText;

          LotPerModel model = new LotPerModel();
            model.dateline= doc.ChildNodes[1].ChildNodes[0].Attributes["dateline"].Value;
            model.LotPer= doc.ChildNodes[1].ChildNodes[0].Attributes["id"].Value;
            model.number  = lotCot.Replace(",","");

          return model;


          //List<Dictionary<string, LotPerModel>> lst =   JSONHelp.Instance().FromJson<List<Dictionary<string, LotPerModel>>>(result1);
          //if (errorCode1 == "0")
          //{
          //    Debug.WriteLine("成功");
          //    Debug.WriteLine(newObj1);
          //}
          //else
          //{
          //    //Debug.WriteLine("失败");
          //    Debug.WriteLine(newObj1["error_code"].Value + ":" + newObj1["reason"].Value);
          //}


          //2.高频彩票最新开奖查询
          //string url2 = "http://op.juhe.cn/lottery/query";

          //var parameters2 = new Dictionary<string, string>();

          //parameters2.Add("key", appkey);//你申请的key
          //parameters2.Add("dtype", ""); //返回数据的格式,xml或json，默认json
          //parameters2.Add("spell", ""); //彩票编码

          //string result2 = sendPost(url2, parameters2, "get");

          //JsonObject newObj2 = new JsonObject(result2);
          //String errorCode2 = newObj2["error_code"].Value;

          //if (errorCode2 == "0")
          //{
          //    Debug.WriteLine("成功");
          //    Debug.WriteLine(newObj2);
          //}
          //else
          //{
          //    //Debug.WriteLine("失败");
          //    Debug.WriteLine(newObj2["error_code"].Value + ":" + newObj2["reason"].Value);
          //}

          //return "";

      }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        static string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

       
    }

    public class LotteryModel
    {
        public string LotterNo { get; set; }
        public LotPerModel LotPers {get;set;}
        public Dictionary<string, LotPerModel> dic { get; set; }
    }

    public class LotPerModel
    {
        public string number { get; set; }
        public string dateline { get; set; }
        public string LotPer { get; set; }
    }
}

