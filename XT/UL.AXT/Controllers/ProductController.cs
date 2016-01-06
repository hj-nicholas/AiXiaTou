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
    public class ProductController : Controller
    {
        BLL.Product prod = new Product();
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();

        // GET: Product
        public ActionResult Index(int proType = 1)
        {

            List<ProductModel> lst = prod.GetProducts(proType,0).ToList();
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
            var showOrder = show.GetShowingOrders(product.ProductId,0);

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
            orderDto.IsPay = 0;
            orderDto.UserAddr = addr;
            orderDto.UserPhone = phone;
            orderDto.UserDesc = "";
            orderDto.UserId = userId;
            BLL.Order order = new Order();
            BaseResult br = order.AddOrderDesc(orderDto);


            return Json(br);
        }

        public ActionResult PaySuccess()
        {
            return View();
        }

        public ActionResult PayResult()
        {
            return View();
        }

        public JsonResult GetImagesByType(int type, int periodId)
        {
            return Json("");
        }

        //生成虾头code
        public JsonResult GetShrimpCode(int periodId, int codeNum)
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
            return Json(codes);
        }

        public bool isExistsNo(string code,int periodId)
        {
            BLL.Order order = new Order();
            bool flag= order.IsExistsLottery(code, periodId);
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

        public JsonResult UpdOrderSts(int orderId, int sts)
        {
            BLL.Order order = new Order();
            BaseResult br = order.updOrderSts(orderId, sts);
            return Json(br);
        }

        public ActionResult ShrimpHeadOf(int productId)
        {
            var periods = prod.GetPeriods(productId).ToList();
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

            var showOrder = show.GetShowingOrders(productId,0);
            ViewBag.UploadPath = strUploadPath;

            return View(showOrder);
        }
    }
}