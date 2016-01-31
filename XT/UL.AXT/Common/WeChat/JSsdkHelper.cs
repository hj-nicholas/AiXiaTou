using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.Configuration;


namespace Hoo.Common.WeChat
{
    public class JSSDK
    {
        private string appId= WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret= WebConfigurationManager.AppSettings["AppSecret"];


        public JSSDK()
        {
            //this.appId = appId;
            //this.appSecret = appSecret;
        }


        //创建Json序列化 及反序列化类目  
        #region  
        //创建JSon类 保存文件 jsapi_ticket.json  
        [DataContract(Namespace = "http://coderzh.cnblogs.com")]
        class JSTicket
        {
            [DataMember(Order = 0)]
            public string jsapi_ticket { get; set; }
            [DataMember(Order = 1)]
            public double expire_time { get; set; }
        }
        //创建 JSon类 保存文件 access_token.json  
        [DataContract(Namespace = "http://coderzh.cnblogs.com")]
        class AccToken
        {
            [DataMember(Order = 0)]
            public string access_token { get; set; }
            [DataMember(Order = 1)]
            public double expire_time { get; set; }
        }


        //创建从微信返回结果的一个类 用于获取ticket  
        [DataContract(Namespace = "http://coderzh.cnblogs.com")]
        class Jsapi
        {
            [DataMember(Order = 0)]
            public int errcode { get; set; }
            [DataMember(Order = 1)]
            public string errmsg { get; set; }
            [DataMember(Order = 2)]
            public string ticket { get; set; }
            [DataMember(Order = 3)]
            public string expires_in { get; set; }
        }
        #endregion


        //得到数据包，返回使用页面  
        public System.Collections.Hashtable getSignPackage()
        {



            string jsapiTicket = getJsApiTicket();
            string url = HttpContext.Current.Request.Url.ToString(); //"http://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";  
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();

            
            // 这里参数的顺序要按照 key 值 ASCII 码升序排序  
            string rawstring = "jsapi_ticket=" + jsapiTicket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url + "";


            string signature = SHA1_Hash(rawstring);


            System.Collections.Hashtable signPackage = new System.Collections.Hashtable();
            signPackage.Add("appId", appId);
            signPackage.Add("nonceStr", nonceStr);
            signPackage.Add("timestamp", timestamp);
            signPackage.Add("url", url);
            signPackage.Add("signature", signature);
            signPackage.Add("rawString", rawstring);

            UL.AXT.Common.Log.WriteLog("appId:", appId);
            UL.AXT.Common.Log.WriteLog("nonceStr:", nonceStr);
            UL.AXT.Common.Log.WriteLog("timestamp:", timestamp);
            UL.AXT.Common.Log.WriteLog("url:", url);
            UL.AXT.Common.Log.WriteLog("signature:", signature);
            UL.AXT.Common.Log.WriteLog("rawstring:", rawstring);
            return signPackage;
        }


        //创建随机字符串  
        private string createNonceStr()
        {


            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }


        //得到ticket 如果文件里时间 超时则重新获取  
        private string getJsApiTicket()
        {
            // jsapi_ticket 应该全局存储与更新，以下代码以写入到文件中做示例  
            string path = HttpContext.Current.Server.MapPath(@"/jssdk/jsapi_ticket.json");

            //读取文件         
            FileStream file = new FileStream(path, FileMode.Open);
            var serializer = new DataContractJsonSerializer(typeof(JSTicket));
            JSTicket readJSTicket = (JSTicket)serializer.ReadObject(file);
            file.Close();
            string ticket = "";
            if (readJSTicket.expire_time < ConvertDateTimeInt(DateTime.Now))
            {
                string accessToken = getAccessToken();
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + accessToken + "";
                Jsapi api = Desrialize<Jsapi>(new Jsapi(), httpGet(url));




                ticket = api.ticket;
                if (ticket != "")
                {
                    readJSTicket.expire_time = ConvertDateTimeInt(DateTime.Now) + 7000;
                    readJSTicket.jsapi_ticket = ticket;


                    string json = Serialize<JSTicket>(readJSTicket);
                    StreamWriterMetod(json, path);
                }
            }
            else
            {
                ticket = readJSTicket.jsapi_ticket;
            }


            return ticket;
        }


        //得到accesstoken 如果文件里时间 超时则重新获取  
        private string getAccessToken()
        {
            // access_token 应该全局存储与更新，以下代码以写入到文件中做示例  


            string access_token = "";
            string path = HttpContext.Current.Server.MapPath(@"/jssdk/access_token.json");
            FileStream file = new FileStream(path, FileMode.Open);
            var serializer = new DataContractJsonSerializer(typeof(AccToken));
            AccToken readJSTicket = (AccToken)serializer.ReadObject(file);
            file.Close();
            if (readJSTicket.expire_time < ConvertDateTimeInt(DateTime.Now))
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret + "";


                AccToken iden = Desrialize<AccToken>(new AccToken(), httpGet(url));


                access_token = iden.access_token;
                if (access_token != "")
                {
                    iden.expire_time = ConvertDateTimeInt(DateTime.Now) + 7000;
                    iden.access_token = access_token;


                    string json = Serialize<AccToken>(iden);
                    StreamWriterMetod(json, path);
                }
            }
            else
            {
                access_token = readJSTicket.access_token;
            }
            return access_token;
        }


        //发起一个http请球，返回值  
        private string httpGet(string url)
        {
            try
            {


                WebClient MyWebClient = new WebClient();



                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  


                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  


                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              


                //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  


                return pageHtml;


            }


            catch (WebException webEx)
            {


                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }


        //SHA1哈希加密算法  
        public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }


        /// <summary>  
        /// StreamWriter写入文件方法  
        /// </summary>  
        private void StreamWriterMetod(string str, string patch)
        {
            try
            {
                FileStream fsFile = new FileStream(patch, FileMode.OpenOrCreate);
                StreamWriter swWriter = new StreamWriter(fsFile);
                //?入??  
                swWriter.WriteLine(str);
                swWriter.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>  
        /// 反序列化 字符串到对象  
        /// </summary>  
        /// <param name="obj">泛型对象</param>  
        /// <param name="str">要转换为对象的字符串</param>  
        /// <returns>反序列化出来的对象</returns>  
        public static T Desrialize<T>(T obj, string str)
        {
            try
            {
                var mStream = new MemoryStream(Encoding.Default.GetBytes(str));
                var serializer = new DataContractJsonSerializer(typeof(T));
                T readT = (T)serializer.ReadObject(mStream);


                return readT;
            }
            catch (Exception ex)
            {
                return default(T);
                throw new Exception("反序列化失败,原因:" + ex.Message);
            }

        }


        /// <summary>  
        /// 序列化 对象到字符串  
        /// </summary>  
        /// <param name="obj">泛型对象</param>  
        /// <returns>序列化后的字符串</returns>  
        public static string Serialize<T>(T obj)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, obj);


                byte[] dataBytes = new byte[stream.Length];


                stream.Position = 0;


                stream.Read(dataBytes, 0, (int)stream.Length);


                string dataString = Encoding.UTF8.GetString(dataBytes);
                return dataString;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }




        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }
    }
}