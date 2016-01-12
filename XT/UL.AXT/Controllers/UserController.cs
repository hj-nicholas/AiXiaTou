using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Hoo.WeChat.WxPayAPI;
using Model;

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
            return View(accInfo);
        }

        public ActionResult Joined(int userId)
        {
            List<ProductModel> lst = prod.GetProducts(1, userId).ToList();
            return View(lst);
        }

        public ActionResult Gifts(int userId)
        {
            var sendGift= user.GetSendGifyByUserId(userId);
            var revGift = user.GetRevGifyByUserId(userId);
            ViewBag.RevGift = revGift;
            return View(sendGift);
        }

        public ActionResult Recharge()
        {
            return View();
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
            return View("User/AccountDetail?userId="+userId);
        }
    }
}