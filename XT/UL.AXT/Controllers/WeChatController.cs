using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using Hoo.Common.WeChat;
using UL.AXT.Models;
using UL.AXT.Common;

namespace UL.AXT.Controllers
{
    public class WeChatController : Controller
    {
        public static readonly string DefaultToken = WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];//与微信公众账号后台的AppId设置保持一致，区分大小写。

        // GET: WeChat
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel)
        {

            //Log.WriteLog("WeChat params Signature:", postModel.Signature);
            //Log.WriteLog("WeChat params Timestamp:", postModel.Timestamp);

            //Log.WriteLog("WeChat params Nonce:", postModel.Nonce);
            //Log.WriteLog("WeChat params echoStr:", echoStr);

            bool flag = Utility.CheckSignature(postModel.Signature, postModel.Timestamp, postModel.Nonce, DefaultToken);
            if (flag)
            {
                return Content(postModel.echoStr);
            }
            else
            {
                return Content("failed:如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
            }


        }


        [HttpPost]
        public ActionResult Index()
        {
            string result = ProcessRequest();
            return Content(result);
        }

        public string ProcessRequest()
        {
            string result = string.Empty;
            //获取流
           HttpRequest request = System.Web.HttpContext.Current.Request;
           
            result = HandlePost(request);
           return result;
        }

        //验证是否微信服务端请求
        public bool Validate(PostModel postModel)
        {
            bool flag = Utility.CheckSignature(postModel.Signature, postModel.Timestamp, postModel.Nonce, DefaultToken);
            return flag;
        }

        /// <summary>
        /// 处理微信的POST请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns>返回xml响应</returns>
        private string HandlePost(HttpRequest request)
        {
            RequestMessageHelper helper = new RequestMessageHelper(request);
            if (helper.Message != null)
            {
                //Message.Insert(new Message(MessageType.Request, helper.Message.ToString()));
                ResponseBaseMessage responseMessage = HandleRequestMessage(helper.Message);
                //Message.Insert(new Message(MessageType.Response, responseMessage.ToString()));
                return responseMessage.ToXml(helper.EncryptType);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 处理请求消息，返回响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        private ResponseBaseMessage HandleRequestMessage(RequestBaseMessage requestMessage)
        {
            ResponseTextMessage response = new ResponseTextMessage(requestMessage.FromUserName, requestMessage.ToUserName,
                DateTime.Now, string.Format("自动回复，请求内容如下：\r\n{0}", requestMessage));
            //response.Content += "\r\n<a href=\"http://xrwang.cnblogs.com\">博客园</a>";
            //ErrorMessage errorMessage = CustomerService.SendMessage(new ResponseTextMessage(requestMessage.FromUserName, requestMessage.ToUserName, DateTime.Now, string.Format("自动回复客服消息，请求内容如下：\r\n{0}", requestMessage.ToString())));
            //if (!errorMessage.IsSuccess)
            //    Message.Insert(new Message(MessageType.Exception, errorMessage.ToString()));
            return response;
        }

        public void Test()
        {
            PostModel postModel = new PostModel();
            postModel.Signature = "08253564366e3c5c4bfaa7ad6cf9e3dc381ad6a0";
            postModel.Timestamp = "1450267413";
            postModel.Nonce = "937523504";

            bool flag = Utility.CheckSignature(postModel.Signature, DefaultToken, postModel.Timestamp, postModel.Nonce );

        }

    }
}