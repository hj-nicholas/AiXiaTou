using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Hoo.WeChat.WxPayAPI
{
    public class PayForWeiXinHelp
    {

        /// <summary>
        /// 调用微信支付接口前处理数据，包括sign验证等
        /// </summary>
        /// <param name="payForWeiXin"></param>
        /// <returns></returns>
        public string DoDataForPayWeiXin(PayWeiXin payForWeiXin)
        {
            #region 处理nonce_str随机字符串，不长于 32 位（本程序生成长度为16位的）
            string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            payForWeiXin.nonce_str = RandomStr(str,16);
            #endregion

            #region 商户信息从config文件中读取

            //商户支付密钥key
            string key = WxPayConfig.KEY;
            //商户号
            payForWeiXin.mch_id = WxPayConfig.MCHID;
            //商户 appid 
            payForWeiXin.wxappid = WxPayConfig.APPID;
            //提供方名称 
            payForWeiXin.nick_name = WxPayConfig.NICKNAME;
            payForWeiXin.act_id = "act_id";
            //红包収送者名称 
            payForWeiXin.send_name = WxPayConfig.SENDNAME;
            //红包収放总人数
            payForWeiXin.total_num = 1;
            //红包祝福诧
            payForWeiXin.wishing = "恭喜发财";
            //活劢名称 
            payForWeiXin.act_name = "提现";
            //备注信息 
            payForWeiXin.remark = "测试";
            //商户logo的url 
            payForWeiXin.logo_imgurl = "";
            //分享文案 
            payForWeiXin.share_content = "";
            //分享链接
            payForWeiXin.share_url = "";
            //分享的图片url 
            payForWeiXin.share_imgurl = "";
            //调用接口的机器 Ip 地址
            payForWeiXin.client_ip = "127.0.0.1";
            #endregion

            #region 订单信息
            //生成订单号组成： mch_id+yyyymmdd+10 位一天内不能重复的数字
            //生成10位不重复的数字
            string num= "0123456789";
            string randomNum = RandomStr(num,10);
            payForWeiXin.mch_billno = payForWeiXin.mch_billno + System.DateTime.Now.ToString("yyyyMMdd") + randomNum;
            #endregion

            string postData = @"<xml> 
                                 <mch_billno>{0}</mch_billno> 
                                 <mch_id>{1}</mch_id> 
                                 <wxappid>{2}</wxappid> 
                                 <nick_name>{3}</nick_name> 
                                 <send_name>{4}</send_name> 
                                 <re_openid>{5}</re_openid> 
                                 <total_amount>{6}</total_amount> 
                                 <min_value>{7}</min_value> 
                                 <max_value>{8}</max_value> 
                                 <total_num>{9}</total_num> 
                                 <wishing>{10}</wishing> 
                                 <client_ip>{11}</client_ip> 
                                 <act_name>{12}</act_name> 
                                 <act_id>{13}</act_id> 
                                 <remark>{14}</remark>
                                 <nonce_str>{15}</nonce_str>";
            postData = string.Format(postData,
                                            payForWeiXin.mch_billno,
                                            payForWeiXin.mch_id,
                                            payForWeiXin.wxappid,
                                            payForWeiXin.nick_name,
                                            payForWeiXin.send_name,
                                            payForWeiXin.re_openid,
                                            payForWeiXin.total_amount,
                                            payForWeiXin.min_value,
                                            payForWeiXin.max_value,
                                            payForWeiXin.total_num,
                                            payForWeiXin.wishing,
                                            payForWeiXin.client_ip,
                                            payForWeiXin.act_name,
                                            payForWeiXin.act_id,
                                            payForWeiXin.remark,
                                            payForWeiXin.nonce_str
                                );


            //原始传入参数
            string[] signTemp = { "mch_billno=" + payForWeiXin.mch_billno, "mch_id=" + payForWeiXin.mch_id, "wxappid=" + payForWeiXin.wxappid, "nick_name=" + payForWeiXin.nick_name, "send_name=" + payForWeiXin.send_name, "re_openid=" + payForWeiXin.re_openid, "total_amount=" + payForWeiXin.total_amount, "min_value=" + payForWeiXin.min_value, "max_value=" + payForWeiXin.max_value, "total_num=" + payForWeiXin.total_num, "wishing=" + payForWeiXin.wishing, "client_ip=" + payForWeiXin.client_ip, "act_name=" + payForWeiXin.act_name, "act_id=" + payForWeiXin.act_id, "remark=" + payForWeiXin.remark, "nonce_str=" + payForWeiXin.nonce_str };

            List<string> signList = signTemp.ToList();

            //拼接原始字符串
            if (!string.IsNullOrEmpty(payForWeiXin.logo_imgurl))
            {
                postData += "<logo_imgurl>{0}</logo_imgurl> ";
                postData = string.Format(postData, payForWeiXin.logo_imgurl);
                signList.Add("logo_imgurl=" + payForWeiXin.logo_imgurl);
            }
            if (!string.IsNullOrEmpty(payForWeiXin.share_content))
            {
                postData += "<share_content>{0}</share_content> ";
                postData = string.Format(postData, payForWeiXin.share_content);
                signList.Add("share_content=" + payForWeiXin.share_content);
            }
            if (!string.IsNullOrEmpty(payForWeiXin.share_url))
            {
                postData += "<share_url>{0}</share_url> ";
                postData = string.Format(postData, payForWeiXin.share_url);
                signList.Add("share_url=" + payForWeiXin.share_url);
            }
            if (!string.IsNullOrEmpty(payForWeiXin.share_imgurl))
            {
                postData += "<share_imgurl>{0}</share_imgurl> ";
                postData = string.Format(postData, payForWeiXin.share_imgurl);
                signList.Add("share_imgurl=" + payForWeiXin.share_imgurl);
            }

            #region 处理支付签名
            //对signList按照ASCII码从小到大的顺序排序
            signList.Sort();
            
            string signOld = string.Empty;
            string payForWeiXinOld = string.Empty;
            int i = 0;
            foreach(string temp in signList)
            {
                signOld += temp + "&";
                i++;
            }
            signOld = signOld.Substring(0, signOld.Length-1);
            //拼接Key
            signOld += "&key="+key;
            //处理支付签名
            payForWeiXin.sign = Encrypt(signOld).ToUpper();
            #endregion
            postData += "<sign>{0}</sign></xml>";
            postData = string.Format(postData, payForWeiXin.sign);
            return postData;
        }

        /// <summary>
        /// 调用微信支付接口
        /// </summary>
        /// <param name="payForWeiXin"></param>
        /// <returns></returns>
        public string PayForWeiXin(string postData) 
        {
            string result = string.Empty;
            try
            {
                 result = PostPage("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack", postData);
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        /// <summary>
        /// post微信请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string PostPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                //CerPath证书路径
                string certPath = WxPayConfig.SSLCERT_PATH;
                //证书密码
                string password = WxPayConfig.SSLCERT_PASSWORD;
                X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);

                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                request.ClientCertificates.Add(cert);
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }



        public string RandomStr(string str,int Length)
        {
            string result = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += str[rd.Next(str.Length)];
            }
            return result;
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String Encrypt(String s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }
    }
}
