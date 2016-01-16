using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Hoo.WeChat.WxPayAPI;
using Model;
using UL.AXT.Common;

namespace UL.AXT.Controllers
{
    public class ProductController : Controller
    {
        BLL.Product prod = new Product();
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();

        // GET: Product
        public ActionResult Index(int proType = 1)
        {

            List<ProductModel> lst = prod.GetProducts(proType, 0).ToList();
            return View(lst);
        }

        public ActionResult ProductDetail(int periodId)
        {
            BLL.ShowOrder show = new ShowOrder();

            //1.产品相关信息
            var product = prod.GetProductByPeriodId(periodId);
            //2.该类型产品进行进度
            var periods = prod.GetPeriods(product.ProductId).Take(3).ToList();
            //3.抢购记录
            var order = prod.GetOrderList(periodId).ToList();
            //4.赠送记录
            var donate = prod.GetDonaterList(periodId).ToList();

            //5.晒单记录
            var showOrder = show.GetShowingOrders(product.ProductId, 0,0);

            ViewBag.Product = product;
            ViewBag.Periods = periods;
            ViewBag.Orders = order;
            ViewBag.Donater = donate;
            ViewBag.ShowOrder = showOrder;
            ViewBag.UploadPath = strUploadPath;

            return View(product);
        }

        public ActionResult SubmitOrder(int periodId, int userId = 0)
        {
            //userId = 3;
            var product = prod.GetProductByPeriodId(periodId);
            //用户信息
            BLL.UserInfo bllUser = new UserInfo();
            UserDTO user = bllUser.GetUserInfo(userId);
            List<T_UserAddr> lstAddr = bllUser.GetReceiveAddrs(userId).ToList();

            ViewBag.UserInfo = user;
            ViewBag.Addrs = lstAddr;
            return View(product);
        }

        public ActionResult SubmitOrderS(int periodId, int userId = 0)
        {
            var product = prod.GetProductByPeriodId(periodId);

            BLL.UserInfo bllUser = new UserInfo();
            UserDTO user = bllUser.GetUserInfo(userId);
            ViewBag.UserInfo = user;

            return View(product);
        }

        public JsonResult AddOrder(string lottery, string phone, string email, int periodId, string userName, string addr, int userId)
        {
            //发送邮件
            //生成订单
            T_User_Order_Desc orderDto = new T_User_Order_Desc();
            orderDto.IsPay = 0;
            orderDto.LotteryNO = lottery;
            orderDto.OrderEmail = email;
            orderDto.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            orderDto.PeriodId = periodId;
            orderDto.UserAddr = addr;
            orderDto.UserPhone = phone;
            orderDto.UserDesc = "";
            orderDto.UserId = userId;
            BLL.Order order = new Order();
            BaseResult br = order.AddOrderDesc(orderDto);


            return Json(br);
        }

        public JsonResult AddGift(int userId, int peopleNum, int giftNum, int periodId)
        {
            T_User_Share shareDto = new T_User_Share();
            shareDto.IsPay = 0;
            shareDto.PeopleNum = peopleNum;
            shareDto.PeriodId = periodId;
            shareDto.ShareNum = giftNum;
            shareDto.ShareUserId = userId;
            BaseResult result = prod.AddGift(shareDto);

            return Json(result);
        }
        public ActionResult PayOrderSuccess(int orderId)
        {
            BLL.Order order = new Order();
            ProductModel prodModel = new ProductModel();
            BaseResult br = order.updOrderSts(orderId, 1);
            if (br.Succeeded)
                prodModel = prod.GetProdByOrder(orderId);
            return View(prodModel);
        }

        public ActionResult PayGiftSuccess( int shareId)
        {
            ProductModel prodModel = new ProductModel();

            bool flag = true;
            if (Session["UserId"] != null)
            {//分享用户id与接受用户id不一致
                var userId = Convert.ToInt32(Session["UserId"]);
                T_User_Share share = prod.GetShareById(shareId);
                flag = share.ShareUserId == userId ? true : false;
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
                BaseResult br = prod.UpdGiftSts(shareId, 1);
                if (br.Succeeded)
                    prodModel = prod.GetProdByShare(shareId);

                List<T_Share_Get> lst = prod.GetRevGiftByShareId(shareId);
            if (lst.Count > 0)
            {
               T_User_Share userShare=  prod.GetShareById(shareId);
                ViewBag.UserShare = userShare;
                    return View("ShareGiftResult", lst);
            }
                
            }
            return View(prodModel);
        }

        public ActionResult ShareGiftResult()
        {
            return View();
        }

        public ActionResult GetGift(int shareId, string code)
        {
            //HOO Test-3
            UserDTO userDto = new UserDTO();

            //ViewBag.Code = code;
            Common.WeChatBusiness weChat = new Common.WeChatBusiness();
            Hoo.Common.WeChat.UserInfo userInfo = weChat.GetWeChatUser(code);
            //Common.Log.WriteLog("get code:", code);
            if (userInfo != null)
            {
                userDto = weChat.ChangeUserByWeChatInfo(userInfo);
                Session["UserId"] = userDto.UserID;
            }

            //Hoo.Common.WeChat.UserInfo userInfo = new Hoo.Common.WeChat.UserInfo("ooSaOwsnQbC52N-srS25TaEV-DeU");
            //userDto = weChat.ChangeUserByWeChatInfo(userInfo);

            ViewBag.UserInfo = userDto;
            ViewBag.ShareId = shareId;

            //抢虾仔
            BaseResult br= RevGift(shareId, userDto.UserID);

            List<T_Share_Get> lstGet = prod.GetRevGiftByShareId(shareId);
            T_User_Share userShare = prod.GetShareById(shareId);
            ViewBag.UserShare = userShare;

            var lst = lstGet.Where(s=>s.GetUserId==userDto.UserID).ToList();
            if (lst.Count > 0)
                ViewBag.SCode = lst[0].LotNum;
            else
                ViewBag.SCode = "";
            return View(lstGet);
        }

        public BaseResult RevGift(int shareId, int userId)
        {
            BaseResult br = new BaseResult();
            //判断该用户是否领取过该礼物
            if (prod.isRevGift(shareId, userId))
            {
                br.Succeeded = false;
                br.ErrMsg = "已经领取过礼物";
            }
            else
            {
                //查询该礼物分享的信息
                T_User_Share shareDto = prod.GetShareById(shareId);
                //用户接受分享礼物,随机生成礼物数量（一个用户可以同时接受到多个虾仔）
                int restGift = shareDto.ShareNum - shareDto.RevGiftNum - (shareDto.PeopleNum - shareDto.RevPeopleNum)+1;
                Random rd = new Random();
                int revNum = rd.Next(1,restGift);
                string codes = GenerateCode(shareDto.PeriodId, revNum);
                //插入虾仔记录
                br= prod.UpdRevGiftInfo(shareId, userId, revNum, shareDto.PeriodId, codes);
                //br.Succeeded = true;
            }
            return br;
        }

        public ActionResult PayResult()
        {
            return View();
        }

        
        //生成虾头code
        public JsonResult GetShrimpCode(int periodId, int codeNum)
        {
            string codes = GenerateCode(periodId, codeNum);
            return Json(codes);
        }

        //计算用户当期的随机虾仔号码
        public string GenerateCode(int periodId, int codeNum)
        {
            string codes = string.Empty;
            var code = "";
            Random rd = new Random();
            var flag = false;
            //生成不重复的抽奖号码
            for (int i = 0; i < codeNum; i++)
            {
                do
                {
                    code = rd.Next(10000, 100000).ToString();
                } while (isExistsNo(code, periodId));

                codes += code + ",";
            }
            if (codes.Length > 0)
                codes = codes.Substring(0, codes.Length - 1);
            return codes;
        }

        public bool isExistsNo(string code, int periodId)
        {
            BLL.Order order = new Order();
            bool flag = order.IsExistsLottery(code, periodId);
            return flag;
        }

        public ActionResult PayOrder(string lottery, string phone, string email, int needPay, string userOpenId, int orderId)
        {
            ViewBag.Lottery = lottery;
            ViewBag.Phone = phone;
            ViewBag.Email = email;
            ViewBag.NeedPay = needPay;
            ViewBag.OrderId = orderId;

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            //jsApiPay.openid = "ooSaOwsnQbC52N-srS25TaEV-DeU";
            jsApiPay.openid = userOpenId;
            jsApiPay.total_fee = needPay;
            ////JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult("购买虾投", needPay.ToString() + "只", "一元夺宝");
                string wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                ViewBag.WxJSParam = wxJsApiParam;

            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
            return View();
        }

        public ActionResult PayGift(int shareId, int needPay, string userOpenId)
        {
            ViewBag.NeedPay = needPay;
            ViewBag.ShareId = shareId;

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            //jsApiPay.openid = "ooSaOwsnQbC52N-srS25TaEV-DeU";
            jsApiPay.openid = userOpenId;
            jsApiPay.total_fee = needPay;
            ////JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult("购买虾投", needPay.ToString() + "只", "一元夺宝");
                string wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                ViewBag.WxJSParam = wxJsApiParam;

            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
            return View();
        }

        public JsonResult UpdOrderSts(int orderId, int sts)
        {
            BLL.Order order = new Order();
            BaseResult br = order.updOrderSts(orderId, sts);

            return Json(br);
        }

        public JsonResult UpdGiftSts(int shareId, int sts)
        {
            BaseResult br = prod.UpdGiftSts(shareId, sts);
            return Json(br);
        }

        public ActionResult ShrimpHeadOf(int productId)
        {
            var periods = prod.GetProductsByProId(productId);
            return View(periods);
        }

        public ActionResult BuyRecords(int periodId)
        {
            var order = prod.GetOrderList(periodId).ToList();

            return View(order);
        }

        public ActionResult GiftsRecords(int periodId)
        {
            var donate = prod.GetDonaterList(periodId).ToList();

            return View(donate);
        }

        public ActionResult ShowHisOrder(int productId)
        {
            BLL.ShowOrder show = new ShowOrder();

            var showOrder = show.GetShowingOrders(productId, 0,0);
            ViewBag.UploadPath = strUploadPath;

            return View(showOrder);
        }

        public ActionResult AddShowOrder(int periodId)
        {
            ViewBag.Period = periodId;
            return View();
        }

        public ActionResult UpdPic(int period, int picType)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string relPath = "";
            BaseResult result = new BaseResult();
            if (hfc.Count > 0)
            {
                 relPath =  "/CommentPhotos/" + DateTime.Now.ToString("yyyyMMdd")+"/";
                string path = AppDomain.CurrentDomain.BaseDirectory + strUploadPath+ relPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                imgPath = strUploadPath+relPath + hfc[0].FileName;
                string PhysicalPath = Server.MapPath(imgPath);
                hfc[0].SaveAs(PhysicalPath);

                //添加照片
                result = prod.AddPic(period, relPath+ hfc[0].FileName, 1);
            }
            return Json(new { Succeeded=result.Succeeded,errMsg=result.ErrMsg, imgPath1 = imgPath }, "text/html", JsonRequestBehavior.AllowGet);

        }
    }
}