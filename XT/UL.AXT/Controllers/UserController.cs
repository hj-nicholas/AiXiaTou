using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using BLL;
using Hoo.WeChat.WxPayAPI;
using Model;
using Newtonsoft.Json;

namespace UL.AXT.Controllers
{
    public class UserController : Controller
    {
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();
        BLL.UserInfo user = new UserInfo();
        private BLL.Product prod = new Product();
        // GET: User
        public ActionResult Index(int userId=1)
        {
            var userInfo= user.GetUserInfo(userId);
            return View(userInfo);
        }

        public ActionResult ReceiveAddrs(int userId = 1)
        {
            var addrs = user.GetReceiveAddrs(userId);
            return View(addrs);
        }

        public ActionResult HomePage(int userId)
        {
            //用户信息
            //参与信息
            //中奖信息
            //晒单信息
            //回复消息

            var userInfo = user.GetUserInfo(userId);
            List<ProductModel> lst = prod.GetProducts(1,userId).ToList();

            BLL.ShowOrder showOrder = new BLL.ShowOrder();
            List<ShowOrderModel> showOrders = showOrder.GetShowingOrders(0,userId, userId).ToList();

            BLL.Comment comment = new Comment();
            List<CommentDTO> comments= comment.GetCommentListByUserid(userId).ToList();

            ViewBag.UserInfo = userInfo;
            ViewBag.ShowOrders = showOrders;
            ViewBag.UploadPath = strUploadPath;
            ViewBag.Comments = comments;

            return View(lst);
        }

        public ActionResult MyEnshrine(int userId)
        {
            List<ProductModel> lst = prod.GetEnshrineProducts(userId).ToList();
            return View(lst);
            
        }

        public JsonResult EditAddr(T_UserAddr addr, int type)
        {
            BaseResult br = user.EditAddr(addr,type);
            return Json(br);
        }

        public JsonResult GetAddrs(int userId)
        {
            var addrs = user.GetReceiveAddrs(userId);
            return Json(addrs);
        }

        public ActionResult AccountDetail(int userId)
        {
            var userInfo = user.GetUserInfo(userId);
            var accInfo = user.GetAccountByUserId(userId);
            ViewBag.Account = userInfo.AccountBalance;
            ViewBag.UserId = userInfo.UserID;
            return View(accInfo);
        }

        public ActionResult Joined(int userId)
        {
            List<ProductModel> lst = prod.GetProducts(1, userId).ToList();
            ViewBag.UserId = userId;
            return View(lst);
        }

        public ActionResult Gifts(int userId)
        {
            var sendGift= user.GetSendGifyByUserId(userId);
            var revGift = user.GetRevGifyByUserId(userId);
            ViewBag.RevGift = revGift;
            ViewBag.UserId = userId;
            return View(sendGift);
        }

        public ActionResult Recharge(int userId)
        {
            var userInfo = user.GetUserInfo(userId);
            return View(userInfo);
        }

        public ActionResult PayRecharge(int chargeNum, string userOpenId)
        {
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            //jsApiPay.openid = "ooSaOwsnQbC52N-srS25TaEV-DeU";
            jsApiPay.openid = userOpenId;
            jsApiPay.total_fee = chargeNum;
            ////JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult("充值订单", chargeNum.ToString() + "只", "充值订单");
                string wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                ViewBag.WxJSParam = wxJsApiParam;
                ViewBag.ChargeNum = chargeNum;
            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
            return View();
        }

        public ActionResult PayChargeSuccess(int userId,int chargeNum)
        {
            var result = user.RechargeAcc(userId, chargeNum);
            return Redirect("AccountDetail?userId="+userId);
        }

        public ActionResult RevGiftDetail(int shareId,int userId)
        {
            ViewBag.UserInfo = user.GetUserInfo(userId);
            ViewBag.ShareId = shareId;

            //抢虾仔
            //BaseResult br = RevGift(shareId, userDto.UserID);

            List<T_Share_Get> lstGet = prod.GetRevGiftByShareId(shareId);
            T_User_Share userShare = prod.GetShareById(shareId);
            ViewBag.UserShare = userShare;

            var lst = lstGet.Where(s => s.GetUserId == userId).ToList();
            if (lst.Count > 0)
                ViewBag.SCode = lst[0].LotNum;
            else
                ViewBag.SCode = "";
            return View(lstGet);
        }

        public ActionResult SendGiftDetail(int shareId, int userId)
        {
            //页面能够分享给其他用户
            List<T_Share_Get> lstGet = prod.GetRevGiftByShareId(shareId);
            bool flag = true;
            if (Session["UserId"] != null)
            {//分享用户id与接受用户id不一致
                var tempUserId = Convert.ToInt32(Session["UserId"]);
                T_User_Share share = prod.GetShareById(shareId);
                flag = share.ShareUserId == tempUserId ? true : false;
            }
            if (Session["UserId"] == null || !flag)
            {
                //HOO Test-2
                Common.WeChatBusiness weChat = new Common.WeChatBusiness();
                string url = weChat.GetAuthCodeUrl("http://www.ixiatou.cn/Product/GetGift?shareId=" + shareId);
                //string url = "http://localhost:54726/Product/GetGift?shareId=" + shareId;
                Response.Redirect(url);
            }
            else
            {
            

            ViewBag.UserInfo = user.GetUserInfo(userId);
            ViewBag.ShareId = shareId;

            //抢虾仔
            //BaseResult br = RevGift(shareId, userDto.UserID);

            //List<T_Share_Get> lstGet = prod.GetRevGiftByShareId(shareId);
            T_User_Share userShare = prod.GetShareById(shareId);
            ViewBag.UserShare = userShare;

            var lst = lstGet.Where(s => s.GetUserId == userId).ToList();
            if (lst.Count > 0)
                ViewBag.SCode = lst[0].LotNum;
            else
                ViewBag.SCode = "";
            }
            return View(lstGet);
        }

        public ActionResult SharePolite(int shareUserId)
        {
            if (Session["UserId"] != null)
            {
                var loginUser = Convert.ToInt32(Session["UserId"]);
                if (loginUser != shareUserId)
                {
                    //领取虾仔
                    //return View("/Share?shareUserId=" + shareUserId);
                    Response.Redirect("/User/Share?shareUserId=" + shareUserId);
                }
            }
            else
            {
                Common.WeChatBusiness weChat = new Common.WeChatBusiness();
                string url = weChat.GetAuthCodeUrl("http://www.ixiatou.cn/User/Share?shareUserId=" + shareUserId);
                Response.Redirect(url);
            }
            var lst = user.GetRedEnvelopeByUser(shareUserId);
            return View(lst);
        }

        public ActionResult Share(int shareUserId, string code)
        {
            UserDTO userDto = new UserDTO();
            if (string.IsNullOrEmpty(code))
            {
                userDto = user.GetUserInfo(Convert.ToInt32(Session["UserId"]));
            }
            else
            {
                Common.WeChatBusiness weChat = new Common.WeChatBusiness();
                Hoo.Common.WeChat.UserInfo userInfo = weChat.GetWeChatUser(code);
                if (userInfo != null)
                {
                    userDto = weChat.ChangeUserByWeChatInfo(userInfo);
                    Session["UserId"] = userDto.UserID;
                }
            }

            UserDTO shareUser = user.GetUserInfo(shareUserId);
            ViewBag.UserInfo = userDto;
            ViewBag.ShareUserId = shareUserId;
            ViewBag.ShareUser = shareUser;
            return View();
        }

        public JsonResult RevRedEnvelope(int shareUserId, int revUserId)
        {
            BaseResult result = new BaseResult();
            if (revUserId == 0)
            {
                result.Succeeded = true;
                result.ErrMsg = "用户信息丢失，请重新登录";
            }
            else
            {
                result = user.RevShrimp(shareUserId, revUserId);

            }
            return Json(result);
        }

        public JsonResult SaveAward(int userId, int periodId, int type, string awardNo, int addrId)
        {
            BaseResult result = new BaseResult();
            result = user.SaveAward(userId, periodId, type, awardNo, addrId);
            return Json(result);
        }

        public ActionResult Withdraw(int userId)
        {
            UserDTO userDto = user.GetUserInfo(userId);
            return View(userDto);
        }

        public JsonResult TX(int money,string openId,int userId)
        {
            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
            BaseResult br = new BaseResult();
            string result = "";
//                @"

//<xml>
//<return_code><![CDATA[SUCCESS]]></return_code>
//<return_msg><![CDATA[发放成功.]]></return_msg>
//<result_code><![CDATA[SUCCESS]]></result_code>
//<err_code><![CDATA[0]]></err_code>
//<err_code_des><![CDATA[发放成功.]]></err_code_des>
//<mch_billno><![CDATA[0010010404201411170000046545]]></mch_billno>
//<mch_id> 10010404 </mch_id>
//<wxappid><![CDATA[wx6fa7e3bab7e15415]]></wxappid>
//<re_openid><![CDATA[onqOjjmM1tad - 3ROpncN - yUfa6uI]]></re_openid>
//<total_amount> 1 </total_amount>
//<send_listid> 100000000020150520314766074200 </send_listid>
//<send_time> 20150520102602 </send_time>
//</xml>
//";
            PayWeiXin model = new PayWeiXin();
            PayForWeiXinHelp PayHelp = new PayForWeiXinHelp();
            //传入OpenId
            //string openId = context.Request.Form["openId"].ToString();
            //传入红包金额(单位分)
            int amount = money * 100;
            //接叐收红包的用户 用户在wxappid下的openid 
            model.re_openid = openId;
            //付款金额，单位分 
            model.total_amount = amount;
            //最小红包金额，单位分 
            model.min_value = amount;
            //最大红包金额，单位分 
            model.max_value = amount;
            //调用方法
            string postData = PayHelp.DoDataForPayWeiXin(model);
            try
            {
                result = PayHelp.PayForWeiXin(postData);
                //Common.Log.WriteLog("tx:", result);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNode xmlNode = doc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
                }

                if (m_values["result_code"].ToString() == "SUCCESS")
                {
                    //红包提取成功
                    br.Succeeded = true;
                    br.ErrMsg = m_values["return_msg"].ToString();
                    //更新用户账户
                    user.UpdUserAcc(money, userId);
                }
                else if(m_values["result_code"].ToString() == "FAIL")
                {
                    br.Succeeded = false;
                    br.ErrMsg = m_values["err_code_des"].ToString();
                }
                else 
                {
                    br.Succeeded = false;
                    br.ErrMsg = "提现失败";
                }

                //Common.Log.WriteLog("TX result_code:", m_values["result_code"].ToString());
            }
            catch (Exception ex)
            {
                //写日志
                br.Succeeded = false;
                br.ErrMsg = "提现失败";
            }
            
            

            
            return Json(br);
        }

    }
}