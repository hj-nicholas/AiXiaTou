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
            //var result = user.RechargeAcc(userId, chargeNum);
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

        public ActionResult SharePolite(int shareUserId)
        {
            //if (Session["UserId"] != null)
            //{
            //    var loginUser = Convert.ToInt32(Session["UserId"]);
            //    if (loginUser != shareUserId)
            //    {
            //        //领取虾仔
            //        //return View("/Share?shareUserId=" + shareUserId);
            //        Response.Redirect("/User/Share?shareUserId=" + shareUserId);
            //    }
            //}
            //else
            //{
            //    Common.WeChatBusiness weChat = new Common.WeChatBusiness();
            //    string url = weChat.GetAuthCodeUrl("http://www.ixiatou.cn/User/Share?shareUserId=" + shareUserId);
            //    Response.Redirect(url);
            //}
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


            ViewBag.UserInfo = userDto;
            ViewBag.ShareUserId = shareUserId;
            return View();
        }

        public JsonResult RevRedEnvelope(int shareUserId, int revUserId)
        {
            BaseResult result = user.RevShrimp(shareUserId, revUserId);
            return Json(result);
        }

       
    }
}